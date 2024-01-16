using MyBank.API.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyBank.API.Features.WithdrawingFunds;

public record WithdrawFundsRequest([Required] int BankAccountId, [Required] decimal Amount);

public static class WithdrawFunds
{
    internal static async ValueTask HandleAsync(
        WithdrawFundsRequest request,
        IQueryable<BankAccount> querable,
        Func<BankTransaction, ValueTask<int>> addAndSaveAsync)
    {
        var bankAccount = querable.FirstOrDefault(ba => ba.Id == request.BankAccountId);

        if (bankAccount is null || request.Amount <= 0)
        {
            throw new Exception("Bank account not found or invalid amount");
        }

        if (request.Amount > bankAccount.Balance)
        {
            throw new ArgumentOutOfRangeException("Insufficient funds");
        }

        var transaction = new BankTransaction
        {
            BankAccountId = request.BankAccountId,
            Amount = -request.Amount,
            OldBalance = bankAccount.Balance
        };

        bankAccount.Balance += transaction.Amount;

        await addAndSaveAsync(transaction);
    }
}
