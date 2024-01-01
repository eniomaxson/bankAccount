using MyBank.API.Domain;

namespace MyBank.API.Features.DepositingFunds;

public record DepositFundsRequest(int BankAccountId, decimal Amount);

public static class DepositFunds
{
    internal static async ValueTask HandleAsync(
        DepositFundsRequest request,
        IQueryable<BankAccount> querable,
        Func<BankTransaction, ValueTask<int>> addAndSaveAsync)
    {
        var bankAccount = querable.FirstOrDefault(ba => ba.Id == request.BankAccountId);

        if (bankAccount is null)
        {
            throw new Exception("Bank account not found");
        }

        if (request.Amount <= 0)
        {
            throw new InvalidOperationException("Invalid deposit amount");
        }

        var transaction = new BankTransaction
        {
            BankAccountId = request.BankAccountId,
            Amount = request.Amount,
            OldBalance = bankAccount.Balance
        };

        bankAccount.Balance += request.Amount;

        await addAndSaveAsync(transaction);
    }
}