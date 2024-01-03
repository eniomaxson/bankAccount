using MyBank.API.Domain;
using MyBank.API.Features.DepositingFunds;

namespace MyBank.API.Tests.UnitTests;

public class DepositFundsTests
{
    private BankAccount _bankAccount;

    public DepositFundsTests()
    {
        _bankAccount = new BankAccount
        {
            Id = 1,
            Balance = 0,
            Owner = "Someone"
        };
    }

    [Fact]
    public async Task ValidDepositRequest_ShouldAddFunds()
    {
        // Arrange
        var queryable = new List<BankAccount> { _bankAccount }.AsQueryable();

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Equal(1000, tran.Amount);
            Assert.Equal(0, tran.OldBalance);
            Assert.Equal(1, tran.BankAccountId);

            return ValueTask.FromResult(1);
        };

        var depositFundRequest = new DepositFundsRequest(1, 1000);

        // Act
        await DepositFunds.HandleAsync(depositFundRequest, queryable, addAndSaveAsyncAssert);

        // Assert
        Assert.Equal(1000, _bankAccount.Balance);
    }

    [Fact]
    public void DepositRequestWithNegativeAmount_ShouldThrowsInvalidOperationException()
    {
        var queryable = new List<BankAccount> { _bankAccount }.AsQueryable();

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
    public void DepositRequestWithInvalidBankAccountId_ShouldThrowsArgumentOutOfRangeException()
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
