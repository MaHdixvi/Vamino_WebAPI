using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Configuration;
using Microsoft.Extensions.Options;
using Infrastructure.Persistence.Repositories;
using Core.Application.Contracts;

namespace Infrastructure.Persistence.Extensions
{
    /// <summary>
    /// متدهای گسترشی برای ثبت سرویس‌های زیرساخت مانند دیتابیس و ریپازیتوری‌ها
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// اضافه کردن سرویس‌های Persistence به کانتینر وابستگی
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            // ثبت DbContext با تنظیمات
            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<DatabaseConfig>>().Value;
                options.UseSqlServer(config.ConnectionString, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(config.CommandTimeout);
                    sqlOptions.EnableRetryOnFailure(
                        config.MaxRetryCount,
                        TimeSpan.FromMilliseconds(config.RetryDelayMilliseconds),
                        null);
                });
            });

            // ثبت ریپازیتوری‌ها
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IInstallmentRepository, InstallmentRepository>();
            services.AddScoped<ITransactionLogRepository, TransactionLogRepository>();
            services.AddScoped<ICreditScoreRepository, CreditScoreRepository>();

            return services;
        }
        public static IServiceCollection AddMiddleware<T>(this IServiceCollection services) where T : class
        {
            services.AddTransient<T>();
            return services;
        }
    }
}