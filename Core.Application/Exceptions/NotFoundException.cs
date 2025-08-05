using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Exceptions
{
    /// <summary>
    /// استثنا برای مواردی که داده مورد نظر یافت نشود
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("داده مورد نظر یافت نشد.")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
