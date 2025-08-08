// Vamino_WebAPI/Middleware/LoggingMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Vamino_WebAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var originalBodyStream = context.Response.Body;

            // ثبت اطلاعات درخواست
            _logger.LogInformation(
                "درخواست دریافت شد: {Method} {Scheme}://{Host}{Path} {QueryString}",
                request.Method,
                request.Scheme,
                request.Host,
                request.Path,
                request.QueryString);

            // ایجاد یک Stream موقت برای ثبت بدنه پاسخ
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            var startTime = DateTime.UtcNow;
            await _next(context);
            var duration = DateTime.UtcNow - startTime;

            // بازیابی بدنه پاسخ
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            // ثبت اطلاعات پاسخ
            _logger.LogInformation(
                "پاسخ ارسال شد: {StatusCode} در {Duration}ms",
                context.Response.StatusCode,
                duration.TotalMilliseconds);

            // کپی کردن بدنه به پاسخ اصلی
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}