using Core.Application.DTOs;
using Core.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vamino_WebAPI.Controllers
{
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly ITransactionLogService _transactionLogService;
        private readonly ILoanApplicationService _loanService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            IPaymentProcessor paymentProcessor,
            ITransactionLogService transactionLogService,
            ILoanApplicationService loanService,
            ILogger<PaymentsController> logger)
        {
            _paymentProcessor = paymentProcessor;
            _transactionLogService = transactionLogService;
            _loanService = loanService;
            _logger = logger;
        }
        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequestDto request)
        {
            try
            {
                // اعتبارسنجی اولیه
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // دریافت کاربر جاری
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                request.UserId = userId;

                // پردازش پرداخت
                var result = await _paymentProcessor.ProcessPaymentAsync(request);

                // برگشت نتیجه مناسب بر اساس نوع نتیجه
                if (result.IsSuccess)
                    return Ok(result);

                if (!string.IsNullOrEmpty(result.RedirectUrl))
                    return Ok(result); // یا Redirect(result.RedirectUrl) در صورت نیاز

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در InitiatePayment");
                return StatusCode(500, "خطای سرور در پردازش پرداخت");
            }
        }
        [HttpGet("verify/{trackingCode}")]
        public async Task<IActionResult> VerifyPayment(string trackingCode)
        {
            try
            {
                // در واقعیت باید از سرویس جداگانه برای تأیید پرداخت استفاده شود
                var paymentService = (IPaymentVerifier)_paymentProcessor;
                var result = await paymentService.VerifyPaymentAsync(trackingCode);

                if (result.Status== "Completed")
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در VerifyPayment برای کد {trackingCode}");
                return StatusCode(500, "خطای سرور در تأیید پرداخت");
            }
        }
        [HttpGet("history/{loanId}")]
        public async Task<IActionResult> GetPaymentHistory(string loanId)
        {
            try
            {
                // اعتبارسنجی دسترسی کاربر به این وام
                var userId = User.FindFirst("sub")?.Value;
                var loan = await _loanService.GetLoanApplicationByIdAsync(loanId);

                if (loan?.UserId != userId)
                    return Forbid();

                var history = await _transactionLogService.GetPaymentHistoryAsync(loanId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در GetPaymentHistory برای وام {loanId}");
                return StatusCode(500, "خطای سرور در دریافت تاریخچه پرداخت");
            }
        }
        [HttpPost("cancel/{trackingCode}")]
        public async Task<IActionResult> CancelPayment(string trackingCode)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                var result = await _paymentProcessor.CancelPaymentAsync(trackingCode, userId);

                return result ? Ok() : BadRequest("امکان لغو پرداخت وجود ندارد");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در CancelPayment برای کد {trackingCode}");
                return StatusCode(500, "خطای سرور در لغو پرداخت");
            }
        }
    }
}
