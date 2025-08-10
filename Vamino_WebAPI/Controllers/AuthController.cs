using Microsoft.AspNetCore.Mvc;
using Infrastructure.Security;
using Shared.Kernel;
using Core.Application.DTOs;

namespace Vamino_WebAPI.Controllers
{
    /// <summary>
    /// کنترلر احراز هویت و مدیریت کاربران
    /// </summary>
    public class AuthController : SiteBaseController
    {
        private readonly UserAuthenticationService _authService;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(UserAuthenticationService authService, JwtTokenGenerator tokenGenerator)
        {
            _authService = authService;
            _tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// ثبت‌نام کاربر جدید
        /// </summary>
        [HttpPost("auth/register")]
        public async Task<ActionResult<Result<string>>> Register([FromBody] UserRegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<string>.Failure(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            try
            {
                var userId = await _authService.RegisterAsync(new Domain.Entities.User
                {
                    Name = model.Name,
                    NationalId = model.NationalId,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    BankAccountNumber = model.BankAccountNumber
                });

                return Ok(Result<string>.Success(userId, "ثبت‌نام با موفقیت انجام شد."));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<string>.Failure(new[] { ex.Message }));
            }
        }

        /// <summary>
        /// ورود کاربر
        /// </summary>
        [HttpPost("auth/login")]
        public async Task<ActionResult<Result<string>>> Login([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<string>.Failure(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            try
            {
                var token = await _authService.LoginAsync(model.PhoneNumber, model.VerificationCode);
                return Ok(Result<string>.Success(token, "ورود با موفقیت انجام شد."));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<string>.Failure(new[] { ex.Message }));
            }
        }
    }
}