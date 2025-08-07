using Core.Application.DTOs;
using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Kernel;

namespace Vamino_WebAPI.Controllers
{
    /// <summary>
    /// کنترلر مدیریت اعلان‌ها
    /// </summary>
    public class NotificationController : SiteBaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// ارسال یک اعلان به کاربر
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Result>> Send([FromBody] NotificationDto notification)
        {
            try
            {
                await _notificationService.SendNotificationAsync(notification);
                return Ok(Result.Success("اعلان با موفقیت ارسال شد."));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(new[] { ex.Message }));
            }
        }

        /// <summary>
        /// ارسال یادآوری پرداخت قسط
        /// </summary>
        [HttpPost("reminder")]
        public async Task<ActionResult<Result>> SendReminder([FromBody] InstallmentReminderDto reminder)
        {
            try
            {
                await _notificationService.SendInstallmentReminderAsync(
                    reminder.UserId, reminder.InstallmentId, reminder.DueDate);
                return Ok(Result.Success("یادآوری با موفقیت ارسال شد."));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(new[] { ex.Message }));
            }
        }
    }
}