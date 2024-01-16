using MyBank.API.Domain;
using Serilog;
using System.ComponentModel.DataAnnotations;
namespace MyBank.API.Features.OpeningBankAccount;

record OpenBankAccountRequest([Required] string Owner);

internal static class OpenBankAccount
{
    internal static async Task HandleAsync(OpenBankAccountRequest request, Func<BankAccount, ValueTask<int>> addAndSaveAsync)
    {
        var bankAccount = new BankAccount
        {
            Owner = request.Owner,
        };

        Log.Information("Open Bank Account received: {bankAccountRequest}", request);

        await addAndSaveAsync(bankAccount);
    }
}
