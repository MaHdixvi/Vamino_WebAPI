using System;

namespace Infrastructure.Persistence.Configuration
{
    /// <summary>
    /// تنظیمات اتصال و رفتار پایگاه داده سیستم
    /// </summary>
    public class DatabaseConfig
    {
        /// <summary>
        /// رشته اتصال به پایگاه داده SQL Server
        /// </summary>
        public string ConnectionString { get; set; } = ""; // مقدار پیش‌فرض حذف شد تا فقط از appsettings خوانده شود

        /// <summary>
        /// نام پایگاه داده
        /// </summary>
        public string DatabaseName { get; set; } = "";

        /// <summary>
        /// نام سرور پایگاه داده
        /// </summary>
        public string ServerName { get; set; } = "";

        /// <summary>
        /// حداکثر تعداد اتصالات به دیتابیس
        /// </summary>
        public int MaxConnectionPoolSize { get; set; } = 100;

        /// <summary>
        /// زمان انتظار برای اتصال به دیتابیس (به ثانیه)
        /// </summary>
        public int ConnectionTimeout { get; set; } = 30;

        /// <summary>
        /// زمان انتظار برای اجرای دستورات (به ثانیه)
        /// </summary>
        public int CommandTimeout { get; set; } = 60;

        /// <summary>
        /// فعال بودن حالت اسکالینگ خودکار
        /// </summary>
        public bool EnableAutoScaling { get; set; } = true;

        /// <summary>
        /// فعال بودن حالت خواندن از Replica (Read-Only)
        /// </summary>
        public bool EnableReadReplica { get; set; } = true;

        /// <summary>
        /// رشته اتصال به Replica برای عملیات خواندن
        /// </summary>
        public string ReadReplicaConnectionString { get; set; } = "";

        /// <summary>
        /// فعال بودن رمزگذاری ارتباط با دیتابیس
        /// </summary>
        public bool EncryptConnection { get; set; } = true;

        /// <summary>
        /// فعال بودن تراکنش‌های توزیع‌شده
        /// </summary>
        public bool EnableDistributedTransactions { get; set; } = false;

        /// <summary>
        /// حداکثر تعداد تلاش برای اجرای موفقیت‌آمیز دستورات
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// فاصله زمانی بین تلاش‌های مجدد (به میلی‌ثانیه)
        /// </summary>
        public int RetryDelayMilliseconds { get; set; } = 200;
    }
}