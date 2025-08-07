using Application.Services;
using Core.Application.Contracts;
using Core.Application.Contracts.Messaging;
using Core.Application.Services;
using Core.Applicationn.Services;
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
using System;
using Vamino_WebAPI.Middleware;


        
 var builder = WebApplication.CreateBuilder(args);

            // ثبت تنظیمات
            builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection("Database"));
            builder.Services.Configure<SecurityConfig>(builder.Configuration.GetSection("Security"));
            builder.Services.Configure<MessagingConfig>(builder.Configuration.GetSection("Messaging"));

            // ثبت سرویس‌های زیرساخت
            builder.Services.AddPersistenceServices();

            // ثبت سرویس‌های امنیتی
            builder.Services.AddScoped<UserAuthenticationService>();
            builder.Services.AddScoped<RoleBasedAccessControl>();
            builder.Services.AddScoped<JwtTokenGenerator>();

            // ثبت سرویس‌های پیام‌رسانی
            builder.Services.AddScoped<ISmsSender, SmsSender>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<INotificationManager, NotificationManager>();

            // ثبت سرویس‌های اصلی
            builder.Services.AddScoped<ILoanApplicationService, LoanProcessingService>();
            builder.Services.AddScoped<ICreditScoreCalculator, CreditScoreCalculator>();
            builder.Services.AddScoped<IPaymentProcessor, PaymentService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<InstallmentManagementService>();

            // ثبت سرویس‌های دیگر
            builder.Services.AddScoped<CommissionCalculator>();

            // ثبت میدلورها
            builder.Services.AddMiddleware<LoggingMiddleware>();
            builder.Services.AddMiddleware<ExceptionHandlingMiddleware>();

            // ثبت API
            builder.Services.AddControllers();

            // ثبت Swagger (در محیط توسعه)
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen();
            }

            var app = builder.Build();

            // تنظیمات Pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.MapControllers();

            // اجرای مایگریشن‌ها در صورت نیاز
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    context.Database.Migrate(); // اعمال مایگریشن‌ها
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"خطای اعمال مایگریشن: {ex.Message}");
                }
            }

            app.Run();
