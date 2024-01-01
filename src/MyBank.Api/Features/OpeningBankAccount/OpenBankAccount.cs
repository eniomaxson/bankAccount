using MyBank.API.Domain;
namespace MyBank.API.Features.OpeningBankAccount;

public record OpenBankAccountRequest(string Owner);

public static class OpenBankAccount
{
    internal static async Task HandleAsync(
    OpenBankAccountRequest bankAccountRequest,
    Func<BankAccount, ValueTask<int>> addAndSaveAsync)
    {
        var bankAccount = new BankAccount
        {
            Owner = bankAccountRequest.Owner,
        };

        await addAndSaveAsync(bankAccount);
    }

}
