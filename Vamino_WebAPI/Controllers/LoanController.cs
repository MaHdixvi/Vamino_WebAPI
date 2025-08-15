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
        public async Task<ActionResult<Result<LoanApplicationDTO>>> Post([FromBody] LoanRequestDto loanRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<LoanApplicationDTO>.Failure(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            try
            {
                var result = await _loanApplicationService.CreateLoanApplicationAsync(loanRequest);
                return Ok(Result<LoanApplicationDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<LoanApplicationDTO>.Failure(new[] { ex.Message }));
            }
        }

        /// <summary>
        /// دریافت جزئیات یک درخواست وام
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<LoanApplicationDTO>>> Get(string id)
        {
            try
            {
                var result = await _loanApplicationService.GetLoanApplicationByIdAsync(id);
                if (result == null)
                {
                    return NotFound(Result<LoanApplicationDTO>.Failure("درخواست وام یافت نشد."));
                }
                return Ok(Result<LoanApplicationDTO>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<LoanApplicationDTO>.Failure(new[] { ex.Message }));
            }
        }
        /// <summary>
        /// دریافت تمام درخواست‌های وام یک کاربر
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Result<List<LoanApplicationDTO>>>> GetByUser(string userId)
        {
            try
            {
                var loans = await _loanApplicationService.GetLoanApplicationsByUserIdAsync(userId);
                return Ok(Result<IEnumerable<LoanApplicationDTO>>.Success(loans));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<List<LoanApplicationDTO>>.Failure(new[] { ex.Message }));
            }
        }
    }
}