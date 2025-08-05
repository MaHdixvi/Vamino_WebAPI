using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Exceptions
{
    /// <summary>
    /// استثنا برای خطاها در منطق کسب‌وکار
    /// </summary>
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException() : base("خطایی در منطق کسب‌وکار رخ داده است.")
        {
        }

        public BusinessLogicException(string message) : base(message)
        {
        }

        public BusinessLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
