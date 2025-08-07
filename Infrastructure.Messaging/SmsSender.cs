using Core.Application.Contracts.Messaging;
using Kavenegar;
using Kavenegar.Exceptions;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    /// <summary>
    /// پیاده‌سازی واقعی ارسال پیامک با استفاده از سرویس Kavenegar
    /// </summary>
    public class SmsSender : ISmsSender
    {
        private readonly string _apiKey;
        private readonly string _senderNumber;

        public SmsSender(string apiKey, string senderNumber = "100000000000")
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _senderNumber = senderNumber;
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                var api = new KavenegarApi(_apiKey);
                var result = await Task.Run(() => api.Send(_senderNumber, phoneNumber, message));

                if (result.Status != 200)
                {
                    throw new Exception($"ارسال پیامک با خطا مواجه شد. کد خطا: {result.Status}");
                }
            }
            catch (ApiException ex)
            {
                // خطای سطح API (مثلاً کلید نامعتبر)
                throw new Exception($"خطای API Kavenegar: {ex.Message}", ex);
            }
            catch (HttpException ex)
            {
                // خطای شبکه یا ارتباط با سرور
                throw new Exception($"خطای ارتباط با سرور Kavenegar: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // خطاهای عمومی
                throw new Exception($"خطای ناشناخته در ارسال پیامک: {ex.Message}", ex);
            }
        }
    }
}