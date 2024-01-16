using MyBank.API.Domain;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace MyBank.API.Features.DepositingFunds;

public record DepositFundsRequest([Required] int BankAccountId, [Required] decimal Amount);

internal static class DepositFunds
{
    internal static async ValueTask HandleAsync(
        DepositFundsRequest request,
        IQueryable<BankAccount> querable,
        Func<BankTransaction, ValueTask<int>> addAndSaveAsync)
    {
        var bankAccount = querable.FirstOrDefault(ba => ba.Id == request.BankAccountId)
            ?? throw new Exception("Bank account not found");

        if (request.Amount <= 0)
        {
            throw new InvalidOperationException("Invalid deposit amount");
        }

        Log.Information("Deposit funds received {@request}", request);

        BankTransaction transaction = new()
        {
            BankAccountId = request.BankAccountId,
            Amount = request.Amount,
            OldBalance = bankAccount.Balance
        };

        bankAccount.Balance += request.Amount;

        await addAndSaveAsync(transaction);
    }
}