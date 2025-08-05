using System;

namespace Shared.Kernel
{
    /// <summary>
    /// استثنا برای خطاها در اعتبارسنجی ورودی‌ها
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException() : base("خطای اعتبارسنجی رخ داده است.")
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}