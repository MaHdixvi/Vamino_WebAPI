using Shared.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// نمره اعتباری کاربر بر اساس سابقه مالی و رفتار کاربری
    /// </summary>
    public class CreditScore:BaseEntity
    {
        public string UserId { get; set; }
        public int Score { get; set; } // مثلاً بین 300 تا 850
        public string RiskLevel { get; set; } // Low, Medium, High
        public DateTime EvaluationDate { get; set; }
        public string ReasonForScore { get; set; } // دلیل نمره (مثلاً تأخیر در پرداخت)
        public string EvaluatedBy { get; set; } // توسط چه کسی ارزیابی شده (AI یا مدیر)
        public User User { get; set; }
    }
}
