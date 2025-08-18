using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.Implementations
{
    public class PaymentValidator : IPaymentValidator
    {
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentMethodRepository;
        private readonly IRecipientService _recipientService;
        private readonly ILogger<PaymentValidator> _logger;

        public PaymentValidator(
            IUserRepository userRepository,
            IPaymentRepository paymentMethodRepository,
            IRecipientService recipientService,
            ILogger<PaymentValidator> logger)
        {
            _userRepository = userRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _recipientService = recipientService;
            _logger = logger;
        }

        public async Task<PaymentValidationResult> ValidateAsync(PaymentRequestDto paymentRequest)
        {
            var result = new PaymentValidationResult();

            try
            {
                // اعتبارسنجی اولیه
                if (paymentRequest == null)
                {
                    return PaymentValidationResult.Invalid("درخواست پرداخت نامعتبر است");
                }

                // اعتبارسنجی کاربر
                var user = await _userRepository.GetByIdAsync(paymentRequest.UserId);
                if (user == null)
                {
                    result.ValidationErrors.Add("UserId", "کاربر یافت نشد");
                }

                // اعتبارسنجی مبلغ
                var amountValidation = await ValidateAmountAsync(paymentRequest.Amount, paymentRequest.Currency);
                if (!amountValidation)
                {
                    result.ValidationErrors.Add("Amount", "مبلغ پرداخت نامعتبر است");
                }

                // اعتبارسنجی روش پرداخت
                var paymentMethodValidation = await ValidatePaymentMethodAsync(
                    paymentRequest.Method.ToString(),
                    paymentRequest.UserId);

                if (!paymentMethodValidation)
                {
                    result.ValidationErrors.Add("PaymentMethod", "روش پرداخت نامعتبر است");
                }

                // اعتبارسنجی گیرنده (برای انتقال وجه)
                if (!string.IsNullOrEmpty(paymentRequest.RecipientAccount))
                {
                    var recipientValidation = await ValidateRecipientAsync(paymentRequest.RecipientAccount);
                    if (!recipientValidation)
                    {
                        result.ValidationErrors.Add("RecipientAccount", "گیرنده پرداخت نامعتبر است");
                    }
                }

                // نتیجه نهایی
                result.IsValid = !result.ValidationErrors.Any();
                if (!result.IsValid)
                {
                    result.ErrorMessage = "اعتبارسنجی پرداخت ناموفق بود";
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در اعتبارسنجی پرداخت");
                return PaymentValidationResult.Invalid("خطای سیستمی در اعتبارسنجی پرداخت");
            }
        }

        public async Task<bool> ValidateAmountAsync(decimal amount, string currency)
        {
            try
            {
                // بررسی مبلغ مثبت
                if (amount <= 0)
                    return false;

                // بررسی محدودیت‌های ارزی
                var currencyInfo = await _paymentMethodRepository.GetCurrencyInfoAsync(currency);
                if (currencyInfo == null)
                    return false;

                // بررسی حداقل و حداکثر مبلغ
                if (amount < currencyInfo.MinAmount || amount > currencyInfo.MaxAmount)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در اعتبارسنجی مبلغ {amount} {currency}");
                throw;
            }
        }

        public async Task<bool> ValidatePaymentMethodAsync(string paymentMethod, string userId)
        {
            try
            {
                // بررسی روش پرداخت فعال برای کاربر
                return await _paymentMethodRepository.IsPaymentMethodValidAsync(
                    userId,
                    paymentMethod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در اعتبارسنجی روش پرداخت {paymentMethod}");
                throw;
            }
        }

        public async Task<bool> ValidateRecipientAsync(string recipientAccount)
        {
            try
            {
                // بررسی وجود و معتبر بودن گیرنده
                return await _recipientService.ValidateRecipientAsync(recipientAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطا در اعتبارسنجی گیرنده {recipientAccount}");
                throw;
            }
        }
    }
}