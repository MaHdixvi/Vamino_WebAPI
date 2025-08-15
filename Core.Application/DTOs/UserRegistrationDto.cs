using System.ComponentModel.DataAnnotations;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO برای ثبت‌نام کاربر جدید
    /// </summary>
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "نام کاربر الزامی است.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "کد ملی الزامی است.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید 10 رقم باشد.")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "شماره موبایل الزامی است.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل باید 11 رقم باشد.")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string BankAccountNumber { get; set; }

        /// <summary>
        /// پسورد کاربر
        /// </summary>
        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد.")]
        public string Password { get; set; }
    }
}
