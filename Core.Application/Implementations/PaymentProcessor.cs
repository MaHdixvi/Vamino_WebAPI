using Core.Application.Contracts;
using Core.Application.DTOs;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly IPosTerminal _pos;
        private readonly ITransactionLogRepository _txLogRepo;
        private readonly ILogger<PaymentProcessor> _logger;
        private readonly IPaymentValidator _validator;
        private readonly ITransactionLogger _transactionLogger;
        private readonly PaymentStrategyFactory _strategyFactory;

        public PaymentProcessor(
            IPaymentValidator validator,
            ITransactionLogger transactionLogger,
            PaymentStrategyFactory strategyFactory,
            IPosTerminal pos,
            ITransactionLogRepository txLogRepo,
            ILogger<PaymentProcessor> logger)
        {
            _validator = validator;
            _transactionLogger = transactionLogger;
            _strategyFactory = strategyFactory;
            _pos = pos;
            _txLogRepo = txLogRepo;
            _logger = logger;
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
        public async Task<PaymentResult> ProcessPosPaymentAsync(PaymentRequestDto request)
        {
            if (!await _pos.IsConnectedAsync())
                await _pos.ConnectAsync();

            var saleResult = await _pos.SaleAsync(new PosSaleRequest(
                request.Amount,
                "IRR",
                request.TrackingCode
            ));

            return new PaymentResult
            {
                IsSuccess = saleResult.Success,
                TrackingCode = request.TrackingCode,
                TransactionId = saleResult.RRN,
                Message = saleResult.Message,
            };
        }
        public async Task<PaymentResult> PayWithPosAsync(PaymentRequestDto request, string trackingCode)
        {
            try
            {
                // لاگ شروع
                await _txLogRepo.AddAsync(new TransactionLog
                {
                    Id = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.UtcNow,
                    Action = "POS_SALE_REQUEST",
                    RelatedEntity = "Loan/Installment",
                    EntityId = request.DeviceId,
                    UserId = request.UserId,
                    Amount = request.Amount,
                    TrackingCode = trackingCode,
                    PaymentMethod = "POS",
                    IPAddress = request.UserIP ?? ""
                });

                var result = await _pos.SaleAsync(new PosSaleRequest(request.Amount, request.Currency ?? "IRR", trackingCode));

                // لاگ نتیجه
                await _txLogRepo.AddAsync(new TransactionLog
                {
                    Id = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.UtcNow,
                    Action = "POS_SALE_RESULT",
                    RelatedEntity = "Loan/Installment",
                    EntityId = request.DeviceId,
                    UserId = request.UserId,
                    Amount = request.Amount,
                    TrackingCode = trackingCode,
                    PaymentMethod = "POS",
                    Details = result.Message + $" | RRN={result.RRN} | STAN={result.Stan}"
                });
                return new PaymentResult
                {
                    IsSuccess = result.Success,
                    TransactionId = result.RRN ?? result.Stan ?? trackingCode,
                    Message = result.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POS payment failed");
                return new PaymentResult { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<bool> CancelPosAsync(string rrnOrStan, string trackingCode)
        {
            var res = await _pos.CancelAsync(rrnOrStan, trackingCode);
            await _txLogRepo.AddAsync(new TransactionLog
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Action = "POS_VOID_RESULT",
                RelatedEntity = "POS",
                EntityId = rrnOrStan,
                TrackingCode = trackingCode,
                PaymentMethod = "POS",
                Details = res.Message
            });
            return res.Success;
        }
    }
}
