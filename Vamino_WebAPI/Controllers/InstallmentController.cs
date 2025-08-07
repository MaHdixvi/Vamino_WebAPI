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

        public InstallmentController(IInstallmentManagementService installmentService)
        {
            _installmentService = installmentService;
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
    }
}