using System.ComponentModel.DataAnnotations;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO برای ورود کاربر
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// شماره موبایل کاربر
        /// </summary>
        [Required(ErrorMessage = "شماره موبایل الزامی است.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل باید 11 رقم باشد.")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// کد تأیید (کد پیامک شده)
        /// </summary>
        [Required(ErrorMessage = "کد تأیید الزامی است.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "کد تأیید باید 6 رقم باشد.")]
        public string VerificationCode { get; set; }
    }
}