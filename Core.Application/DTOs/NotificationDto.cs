using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO برای ارسال اعلان به کاربر
    /// </summary>
    public class NotificationDto
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // SMS, Email, Push
        public DateTime SendDate { get; set; }
    }
}
