using MyBank.API.Domain;
using MyBank.API.Features.DepositingFunds;

namespace MyBank.API.Tests.UnitTests;

public class DepositFundsTests
{
    private List<BankAccount> _bankAccounts = new() {
        new(){ Id = 1, Balance = 0, Owner = "Someone"},
        new(){ Id = 2, Balance=1000, Owner="Someone else"}
    };

    [Fact]
    public async Task ValidAmmountAndBankAccountId_AddFundsToAccount()
    {
        // Arrange

        var queryable = _bankAccounts.AsQueryable();
        var bankAccount = _bankAccounts.First();

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Equal(1000, tran.Amount);
            Assert.Equal(0, tran.OldBalance);
            Assert.Equal(bankAccount.Id, tran.BankAccountId);

            return ValueTask.FromResult(1);
        };

        var depositFundRequest = new DepositFundsRequest(1, 1000);

        // Act
        await DepositFunds.HandleAsync(depositFundRequest, queryable, addAndSaveAsyncAssert);

        // Assert
        Assert.Equal(1000, bankAccount.Balance);
    }

    [Fact]
    public void NegativeAmount_ThrowsInvalidOperationException()
    {
        var queryable = _bankAccounts.AsQueryable();

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Fail("Should not be called");
            return ValueTask.FromResult(1);
        };

        var depositFundRequest = new DepositFundsRequest(1, 0);

        _ = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await DepositFunds.HandleAsync(depositFundRequest, queryable, addAndSaveAsyncAssert)
        );
    }

    [Fact]
    public void InvalidBankAccountId_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var queryable = new List<BankAccount> { }.AsQueryable();

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Fail("Should not be called");

            return ValueTask.FromResult(1);
        };

        var depositFundRequest = new DepositFundsRequest(1, 0);

        // Act and Assert
        _ = Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            async () => await DepositFunds.HandleAsync(depositFundRequest, queryable, addAndSaveAsyncAssert)
        );
    }
}
