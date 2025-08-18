using Core.Application.Contracts;
using Core.Application.Services;
using Core.Application.utils;

public class PaymentStrategyFactory 
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionLogService _transactionLogService;

    public PaymentStrategyFactory(IWalletRepository walletRepository, ITransactionLogService transactionLogService)
    {
        _walletRepository = walletRepository;
        _transactionLogService = transactionLogService;
    }

    public IPaymentStrategy GetStrategy(string type)
    {
        return type.ToLower() switch
        {
            "wallet" => new WalletPaymentStrategy(_walletRepository, _transactionLogService),
            "bank" => new BankPaymentStrategy(),
            _ => throw new NotImplementedException("استراتژی پرداخت مشخص نشده است")
        };
    }
}
