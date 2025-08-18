using Core.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly IPaymentValidator _validator;
        private readonly ITransactionLogger _transactionLogger;
        private readonly PaymentStrategyFactory _strategyFactory;

        public PaymentProcessor(
            IPaymentValidator validator,
            ITransactionLogger transactionLogger,
            PaymentStrategyFactory strategyFactory)
        {
            _validator = validator;
            _transactionLogger = transactionLogger;
            _strategyFactory = strategyFactory;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequestDto paymentRequest)
        {
            var validation = _validator.ValidateAsync(paymentRequest);
            if (!validation.Result.IsValid)
                return PaymentResult.Failed(validation.Result.ErrorMessage);

            var trackingCode = GenerateTrackingCode();
            var strategy = _strategyFactory.GetStrategy(paymentRequest.Method.ToString());
            var result = await strategy.ProcessAsync(paymentRequest, trackingCode);

            await _transactionLogger.LogPaymentResultAsync(paymentRequest, result);

            return result;
        }

        public async Task<PaymentStatusResult> VerifyPaymentAsync(string trackingCode)
        {
            await Task.Delay(50);
            return new PaymentStatusResult
            {
                TrackingCode = trackingCode,
                Status = "Completed",
                IsVerified = true,
                VerificationDate = DateTime.UtcNow
            };
        }

        public async Task<bool> CancelPaymentAsync(string trackingCode, string userId)
        {
            await Task.Delay(50);
            return true;
        }

        public string GenerateTrackingCode()
        {
            return Guid.NewGuid().ToString("N");
        }

        public BankAccountValidationResult ValidateBankAccount(string bankAccountNumber)
        {
            bool isValid = !string.IsNullOrEmpty(bankAccountNumber) && bankAccountNumber.Length >= 10;
            return new BankAccountValidationResult
            {
                IsValid = isValid,
                BankName = "Sample Bank",
                CardType = "Debit",
                ValidationMessage = isValid ? "حساب معتبر است" : "شماره حساب نامعتبر است"
            };
        }

        public PaymentAmountBreakdown CalculatePaymentAmount(decimal amount, string currency = "IRR")
        {
            decimal tax = amount * 0.09m;
            decimal commission = amount * 0.02m;
            decimal total = amount + tax + commission;

            return new PaymentAmountBreakdown
            {
                OriginalAmount = amount,
                Tax = tax,
                Commission = commission,
                TotalAmount = total
            };
        }

        public async Task<PaymentGatewayResponse> GetPaymentGatewayUrlAsync(PaymentGatewayRequest request)
        {
            await Task.Delay(50);
            return new PaymentGatewayResponse
            {
                Success = true,
                GatewayUrl = $"https://gateway.example.com/pay/{GenerateTrackingCode()}",
                PaymentToken = Guid.NewGuid().ToString("N")
            };
        }

        public async Task<PaymentResult> ProcessGatewayCallbackAsync(PaymentCallbackDto callbackData)
        {
            await Task.Delay(50);
            return PaymentResult.Success(callbackData.TrackingCode, callbackData.TransactionId);
        }
    }
}
