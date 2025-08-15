using Microsoft.AspNetCore.Mvc;
using Infrastructure.Security;
using Shared.Kernel;
using Core.Application.DTOs;
using Domain.Entities;
using Core.Application.Contracts;

namespace Vamino_WebAPI.Controllers
{
    /// <summary>
    /// کنترلر احراز هویت و مدیریت کاربران
    /// </summary>
    public class AuthController : SiteBaseController
    {
        private readonly IUserAuthenticationService _authService;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(IUserAuthenticationService authService, JwtTokenGenerator tokenGenerator)
        {
            _authService = authService;
            _tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// ثبت‌نام کاربر جدید
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<Result<string>>> Register([FromBody] UserRegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<string>.Failure(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            try
            {
                var user = await _authService.RegisterAsync(model);

                return Ok(Result<string>.Success(user.UserId, "ثبت‌نام با موفقیت انجام شد."));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<string>.Failure(new[] { ex.Message }));
            }
        }
        /// <summary>
        /// ویرایش پروفایل کاربر
        /// </summary>
        /// <param name="model">اطلاعات کاربر برای به‌روزرسانی</param>
        /// <returns>نتیجه عملیات به‌روزرسانی</returns>
        [HttpPost("update-profile")]
        public async Task<ActionResult<Result<UserProfileDto>>> UpdateProfile([FromBody] UserProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<UserProfileDto>.Failure(
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var updatedUser = await _authService.UpdateProfileAsync(model);
                return Ok(Result<UserProfileDto>.Success(updatedUser, "پروفایل با موفقیت به‌روزرسانی شد."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(Result<UserProfileDto>.Failure(new[] { "کاربر مورد نظر یافت نشد." }));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(Result<UserProfileDto>.Failure(new[] { "دسترسی غیرمجاز." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Result<UserProfileDto>.Failure(new[] { "خطای سرور در پردازش درخواست." }));
            }
        }
        /// <summary>
        /// ورود کاربر
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<Result<string>>> Login([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<string>.Failure(
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                // تغییر این خط برای استفاده از Username و Password
                var token = await _authService.LoginAsync( model);

                return Ok(Result<string>.Success(token.Token, "ورود با موفقیت انجام شد."));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<string>.Failure(new[] { ex.Message }));
            }
        }

    }
}