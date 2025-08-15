using System.ComponentModel.DataAnnotations;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO برای ورود کاربر
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// نام کاربری
        /// </summary>
        [Required(ErrorMessage = "نام کاربری الزامی است.")]
        [StringLength(50, ErrorMessage = "نام کاربری نباید بیشتر از 50 کاراکتر باشد.")]
        public string Username { get; set; }

        /// <summary>
        /// رمز عبور
        /// </summary>
        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد.")]
        public string Password { get; set; }
    }
}
