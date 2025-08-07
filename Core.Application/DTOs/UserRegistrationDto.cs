using System.ComponentModel.DataAnnotations;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO برای ثبت‌نام کاربر جدید
    /// </summary>
    public class UserRegistrationDto
    {
        /// <summary>
        /// نام کامل کاربر
        /// </summary>
        [Required(ErrorMessage = "نام کاربر الزامی است.")]
        public string Name { get; set; }

        /// <summary>
        /// کد ملی کاربر
        /// </summary>
        [Required(ErrorMessage = "کد ملی الزامی است.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید 10 رقم باشد.")]
        public string NationalId { get; set; }

        /// <summary>
        /// شماره موبایل کاربر
        /// </summary>
        [Required(ErrorMessage = "شماره موبایل الزامی است.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل باید 11 رقم باشد.")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// آدرس ایمیل کاربر
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// شماره حساب بانکی کاربر
        /// </summary>
        public string BankAccountNumber { get; set; }
    }
}