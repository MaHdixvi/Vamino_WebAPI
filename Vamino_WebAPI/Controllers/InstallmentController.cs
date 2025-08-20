using Application.Services;
using Core.Application.DTOs;
using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Kernel;

namespace Vamino_WebAPI.Controllers
{
    /// <summary>
    /// کنترلر مدیریت اقساط وام
    /// </summary>
    public class InstallmentController : SiteBaseController
    {
        private readonly IInstallmentManagementService _installmentService;
        private readonly ILoanApplicationService _loanApplicationService;

        public InstallmentController(IInstallmentManagementService installmentService,ILoanApplicationService loanApplicationService)
        {
            _installmentService = installmentService;
            _loanApplicationService = loanApplicationService;
        }

        /// <summary>
        /// تولید جدول اقساط برای یک وام
        /// </summary>
        [HttpPost("schedule")]
        public async Task<ActionResult<Result<InstallmentScheduleDto>>> GenerateSchedule([FromBody] InstallmentScheduleRequestDto request)
        {
            try
            {
                var installments = await _installmentService.GenerateInstallmentScheduleAsync(
                    request.LoanId, request.Amount, request.NumberOfInstallments);

                var scheduleDto = new InstallmentScheduleDto
                {
                    LoanId = request.LoanId,
                    TotalAmount = request.Amount,
                    NumberOfInstallments = request.NumberOfInstallments,
                    Installments = installments.Select(i => new InstallmentDto
                    {
                        Number = i.Number,
                        DueDate = i.DueDate,
                        PrincipalAmount = i.PrincipalAmount,
                        InterestAmount = i.InterestAmount,
                        TotalAmount = i.TotalAmount,
                        Status = i.Status
                    }).ToList()
                };

                return Ok(Result<InstallmentScheduleDto>.Success(scheduleDto));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<InstallmentScheduleDto>.Failure(new[] { ex.Message }));
            }
        }

        /// <summary>
        /// دریافت اقساط یک وام
        /// </summary>
        [HttpGet("by-loan/{loanId}")]
        public async Task<ActionResult<Result<IEnumerable<InstallmentDto>>>> GetByLoanId(string loanId)
        {
            try
            {
                var installments = await _installmentService.GetInstallmentsByLoanIdAsync(loanId);
                var dtos = installments.Select(i => new InstallmentDto
                {
                    Id=i.Id,
                    Number = i.Number,
                    DueDate = i.DueDate,
                    PrincipalAmount = i.PrincipalAmount,
                    InterestAmount = i.InterestAmount,
                    TotalAmount = i.TotalAmount,
                    Status = i.Status
                });

                return Ok(Result<IEnumerable<InstallmentDto>>.Success(dtos));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<IEnumerable<InstallmentDto>>.Failure(new[] { ex.Message }));
            }
        }
        [HttpPost("pay")]
        public async Task<ActionResult<Result<InstallmentPaymentResult>>> PayInstallment([FromBody] InstallmentPaymentRequest request)
        {
            try
            {
                // فرض بر این که سرویس پرداخت را دارد
                var result = await _installmentService.PayInstallmentAsync(
                    request.LoanId, request.InstallmentNumber, request.Amount, request.PaymentMethod);

                var paymentResult = new InstallmentPaymentResult
                {
                    Success = result.Success,
                    Message = result.Message,
                    TrackingCode = result.TrackingCode
                };

                return Ok(Result<InstallmentPaymentResult>.Success(paymentResult));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<InstallmentPaymentResult>.Failure(new[] { ex.Message }));
            }
        }
        /// <summary>
        /// پرداخت کل اقساط یک وام
        /// </summary>
        [HttpPost("pay-all/{loanId}")]
        public async Task<ActionResult<Result<InstallmentPaymentResult>>> PayAllInstallments(string loanId, [FromBody] InstallmentPaymentRequest request)
        {
            try
            {
                // دریافت تمام اقساط وام
                var installments = await _installmentService.GetInstallmentsByLoanIdAsync(loanId);
                var unpaidInstallments = installments.Where(i => i.Status.ToLower() != "paid").ToList();

                if (!unpaidInstallments.Any())
                    return BadRequest(Result<InstallmentPaymentResult>.Failure(new[] { "تمام اقساط قبلاً پرداخت شده‌اند." }));

                // پرداخت هر قسط به ترتیب
                foreach (var installment in unpaidInstallments)
                {
                    await _installmentService.PayInstallmentAsync(
                        loanId,
                        installment.Number,
                        installment.TotalAmount, // پرداخت کل مبلغ قسط
                        request.PaymentMethod
                    );
                }
                await _loanApplicationService.UpdateLoanApplicationStatusAsync(loanId,"Paid");
                return Ok(Result<InstallmentPaymentResult>.Success(new InstallmentPaymentResult
                {
                    Success = true,
                    Message = $"تمام {unpaidInstallments.Count} قسط با موفقیت پرداخت شد.",
                    TrackingCode = Guid.NewGuid().ToString() // می‌توانید منطق اختصاصی خود برای TrackingCode داشته باشید
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<InstallmentPaymentResult>.Failure(new[] { ex.Message }));
            }
        }

    }
}