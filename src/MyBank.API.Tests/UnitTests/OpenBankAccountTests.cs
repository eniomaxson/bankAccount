using MyBank.API.Domain;
using MyBank.API.Features.OpeningBankAccount;

namespace MyBank.API.Tests.UnitTests;

public class OpenBankAccountTests
{
    [Fact]
    public async Task ValidOpenBnakRequest_ShouldCreateBankAccount()
    {
        var openBankAccountRequest = new OpenBankAccountRequest("Someone");

        var addAndSaveAssert = (BankAccount bank) =>
        {
            Assert.Equal("Someone", bank.Owner);
            Assert.Equal(0, bank.Balance);

            return ValueTask.FromResult(1);
        };

        await OpenBankAccount.HandleAsync(openBankAccountRequest, addAndSaveAssert);
    }
}
