using Shared.Kernel;
using System;

namespace Domain.Entities
{
    public class CurrencyInfo: BaseEntity
    {
        public string CurrencyCode { get; set; }  // مثل IRR, USD, EUR
        public string CurrencyName { get; set; }  // مثلا Rial, Dollar
        public decimal MinAmount { get; set; }    // حداقل مبلغ مجاز
        public decimal MaxAmount { get; set; }    // حداکثر مبلغ مجاز
        public int DecimalPlaces { get; set; }    // تعداد اعشار (مثلا برای ریال = 0، دلار = 2)
        public bool IsActive { get; set; }        // فعال یا غیرفعال بودن ارز
    }
}
