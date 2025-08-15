using Application.Services;
using Core.Application.Contracts;
using Core.Application.Contracts.Messaging;
using Core.Application.Implementations;
using Core.Application.Services;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Configuration;
using Infrastructure.Persistence.Configuration;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Security;
using Infrastructure.Security.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using Vamino_WebAPI.Middleware;

namespace Vamino_WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ثبت تنظیمات
            builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection("Database"));
            builder.Services.Configure<SecurityConfig>(builder.Configuration.GetSection("Security"));
            builder.Services.Configure<MessagingConfig>(builder.Configuration.GetSection("Messaging"));

            // خواندن دامین‌های مجاز از appsettings.json
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new string[0];

            // تعریف CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DynamicCorsPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // ثبت سرویس‌های زیرساخت
            builder.Services.AddPersistenceServices();

            // ثبت سرویس‌های امنیتی
            builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            builder.Services.AddScoped<RoleBasedAccessControl>();

            // ثبت JwtTokenGenerator
            builder.Services.AddScoped<JwtTokenGenerator>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<SecurityConfig>>().Value;
                return new JwtTokenGenerator(
                    config.JwtSecretKey,
                    config.JwtIssuer,
                    config.JwtAudience,
                    config.JwtExpiryMinutes
                );
            });

            // ثبت سرویس‌های پیام‌رسانی
            builder.Services.AddScoped<ISmsSender>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<MessagingConfig>>().Value;
                return new SmsSender(config.SmsApiKey, config.SenderNumber);
            });

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<INotificationManager, NotificationManager>();

            // ثبت سرویس‌های اصلی
            builder.Services.AddScoped<ILoanApplicationService, LoanProcessingService>();
            builder.Services.AddScoped<ICreditScoreCalculator, CreditScoreCalculator>();
            builder.Services.AddScoped<IPaymentProcessor, PaymentService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IInstallmentManagementService, InstallmentManagementService>();
            builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.AddControllers();

            // Swagger
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen();
            }

            var app = builder.Build();

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // فعال کردن CORS قبل از Authorization
            app.UseCors("DynamicCorsPolicy");

            app.UseAuthorization();

            // میان‌افزارها
            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            // مایگریشن‌ها
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"خطای اعمال مایگریشن: {ex.Message}");
                }
            }

            app.Run();
        }
    }
}
