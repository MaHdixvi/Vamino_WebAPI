using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOs
{
    public class CreditScoreResponseDto
    {
        public string UserId { get; set; }
        public int Score { get; set; }
        public string RiskLevel { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string EvaluatedBy { get; set; }
    }
}
