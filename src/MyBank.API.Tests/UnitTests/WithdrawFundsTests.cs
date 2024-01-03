using MyBank.API.Domain;
using MyBank.API.Features.WithdrawingFunds;

namespace MyBank.API.Tests.UnitTests;

public class WithdrawFundsTests
{
    private const int INITIAL_BALANCE = 10000;
    private IQueryable<BankAccount> _queryable;
    private BankAccount _bankAccount;

    public WithdrawFundsTests()
    {
        _bankAccount = new()
        {
            Id = 1,
            Owner = "Tests",
            Balance = INITIAL_BALANCE,
        };

        _queryable = new List<BankAccount> { _bankAccount }.AsQueryable();
    }

    [Fact]
    public async Task ValidWithdrawRequestAndBankWithFunds_ShouldWithdraw()
    {
        // Arrange
        var withdrawAmount = 1000;
        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Equal(-withdrawAmount, tran.Amount);
            Assert.Equal(INITIAL_BALANCE, tran.OldBalance);
            Assert.Equal(_bankAccount.Id, tran.BankAccountId);
            Assert.Equal(DateTime.Now.Date, tran.Date.Date);

            return ValueTask.FromResult(1);
        };
        var withdrawFundsRequest = new WithdrawFundsRequest(_bankAccount.Id, withdrawAmount);

        // Act
        await WithdrawFunds.HandleAsync(withdrawFundsRequest, _queryable, addAndSaveAsyncAssert);

        // Assert
        Assert.Equal(INITIAL_BALANCE - withdrawAmount, _bankAccount.Balance);
    }

    [Fact]
    public void ValidWithdrawRequestAndBankWithoutFunds_ShouldThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var balance = 500;
        var withdrawAmount = 1000;

        _bankAccount.Balance = balance;

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Fail("AddAndSave should not be called.");

            return ValueTask.FromResult(1);
        };

        var withdrawFundsRequest = new WithdrawFundsRequest(_bankAccount.Id, withdrawAmount);

        // Act
        _ = Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            async () => await WithdrawFunds.HandleAsync(withdrawFundsRequest, _queryable, addAndSaveAsyncAssert)
        );

        // Assert
        Assert.Equal(balance, _bankAccount.Balance);
    }

    [Fact]
    public void WithdrawRequestWithInvalidBankAccountId_ShouldTrowsException()
    {
        // Arrange
        var balance = 500;
        var withdrawAmount = 1000;
        var invalidBankAccountId = 10;

        _bankAccount.Balance = balance;

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Fail("AddAndSave should not be called.");

            return ValueTask.FromResult(1);
        };

        var withdrawFundsRequest = new WithdrawFundsRequest(invalidBankAccountId, withdrawAmount);

        // Act
        _ = Assert.ThrowsAsync<Exception>(
            async () => await WithdrawFunds.HandleAsync(withdrawFundsRequest, _queryable, addAndSaveAsyncAssert)
        );

        // Assert
        Assert.Equal(balance, _bankAccount.Balance);
    }

    [Fact]
    public void WithdrawRequestWithNegativeAmount_ShouldThrowsException()
    {
        // Arrange
        var withdrawAmount = -1000;

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Fail("AddAndSave should not be called.");

            return ValueTask.FromResult(1);
        };

        var withdrawFundsRequest = new WithdrawFundsRequest(_bankAccount.Id, withdrawAmount);

        // Act
        _ = Assert.ThrowsAsync<Exception>(
            async () => await WithdrawFunds.HandleAsync(withdrawFundsRequest, _queryable, addAndSaveAsyncAssert)
        );

        // Assert
        Assert.Equal(INITIAL_BALANCE, _bankAccount.Balance);
    }
}