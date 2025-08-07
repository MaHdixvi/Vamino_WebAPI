using Core.Application.DTOs;
using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Kernel;

namespace Vamino_WebAPI.Controllers
{
    /// <summary>
    /// کنترلر مدیریت درخواست‌های وام
    /// </summary>
    public class LoanController : SiteBaseController
    {
        private readonly ILoanApplicationService _loanApplicationService;

        public LoanController(ILoanApplicationService loanApplicationService)
        {
            _loanApplicationService = loanApplicationService;
        }

        /// <summary>
        /// ثبت درخواست وام جدید
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Result<LoanRequestDto>>> Post([FromBody] LoanRequestDto loanRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<LoanRequestDto>.Failure(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            try
            {
                var result = await _loanApplicationService.CreateLoanApplicationAsync(loanRequest);
                return Ok(Result<LoanRequestDto>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<LoanRequestDto>.Failure(new[] { ex.Message }));
            }
        }

        /// <summary>
        /// دریافت جزئیات یک درخواست وام
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<LoanRequestDto>>> Get(string id)
        {
            try
            {
                var result = await _loanApplicationService.GetLoanApplicationByIdAsync(id);
                if (result == null)
                {
                    return NotFound(Result<LoanRequestDto>.Failure("درخواست وام یافت نشد."));
                }
                return Ok(Result<LoanRequestDto>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<LoanRequestDto>.Failure(new[] { ex.Message }));
            }
        }
    }
}