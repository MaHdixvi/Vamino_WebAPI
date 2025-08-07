using Core.Application.DTOs;
using Core.Application.Exceptions;
using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Kernel;

namespace Vamino_WebAPI.Controllers
{
    /// <summary>
    /// کنترلر مدیریت برای کارشناسان بانکی
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILoanApplicationService _loanApplicationService;
        private readonly INotificationService _notificationService;

        public AdminController(
            ILoanApplicationService loanApplicationService,
            INotificationService notificationService)
        {
            _loanApplicationService = loanApplicationService;
            _notificationService = notificationService;
        }

        /// <summary>
        /// دریافت لیست تمام درخواست‌های وام
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<LoanRequestDto>>>> GetAllLoanApplications()
        {
            try
            {
                var applications = await _loanApplicationService.GetAllLoanApplicationsAsync();
                return Ok(Result<IEnumerable<LoanRequestDto>>.Success(applications));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<IEnumerable<LoanRequestDto>>.Failure(new[] { ex.Message }));
            }
        }

        /// <summary>
        /// دریافت جزئیات یک درخواست وام
        /// </summary>
        /// <param name="id">شناسه درخواست</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<LoanRequestDto>>> GetLoanApplication(string id)
        {
            try
            {
                var application = await _loanApplicationService.GetLoanApplicationByIdAsync(id);
                if (application == null)
                {
                    return NotFound(Result<LoanRequestDto>.Failure("درخواست وام یافت نشد."));
                }
                return Ok(Result<LoanRequestDto>.Success(application));
            }
            catch (NotFoundException ex)
            {
                return NotFound(Result<LoanRequestDto>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<LoanRequestDto>.Failure(new[] { ex.Message }));
            }
        }

        /// <summary>
        /// تأیید یا رد درخواست وام توسط مدیر
        /// </summary>
        /// <param name="id">شناسه درخواست</param>
        /// <param name="status">وضعیت (Approved یا Rejected)</param>
        /// <param name="reason">دلیل رد درخواست (اختیاری)</param>
        [HttpPut("{id}/status")]
        public async Task<ActionResult<Result>> UpdateApplicationStatus(string id, [FromQuery] string status, [FromQuery] string reason = null)
        {
            if (string.IsNullOrEmpty(status) || (status != "Approved" && status != "Rejected"))
            {
                return BadRequest(Result.Failure("وضعیت باید Approved یا Rejected باشد."));
            }

            try
            {
                await _loanApplicationService.UpdateLoanApplicationStatusAsync(id, status, reason);

                // ارسال اعلان به کاربر
                var application = await _loanApplicationService.GetLoanApplicationByIdAsync(id);
                if (application != null)
                {
                    await _notificationService.SendLoanApplicationStatusUpdateAsync(application.UserId, status, id);
                }

                return Ok(Result.Success("وضعیت درخواست با موفقیت به‌روزرسانی شد."));
            }
            catch (NotFoundException ex)
            {
                return NotFound(Result.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(new[] { ex.Message }));
            }
        }

        /// <summary>
        /// دریافت لیست اقساط معوقه
        /// </summary>
        [HttpGet("overdue-installments")]
        public async Task<ActionResult<Result<IEnumerable<InstallmentDto>>>> GetOverdueInstallments()
        {
            try
            {
                // در عمل، این متد باید از یک سرویس مدیریت اقساط فراخوانی شود
                // برای سادگی، یک لیست نمونه برمی‌گردانیم
                var overdueInstallments = new List<InstallmentDto>
                {
                    new InstallmentDto
                    {
                        Number = 1,
                        DueDate = DateTime.UtcNow.AddDays(-10),
                        TotalAmount = 15_000_000,
                        Status = "Overdue"
                    }
                };

                return Ok(Result<IEnumerable<InstallmentDto>>.Success(overdueInstallments));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<IEnumerable<InstallmentDto>>.Failure(new[] { ex.Message }));
            }
        }
    }
}