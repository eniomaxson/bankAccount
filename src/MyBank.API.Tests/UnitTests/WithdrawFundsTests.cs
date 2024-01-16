using FluentAssertions;
using MyBank.API.Domain;
using MyBank.API.Features.WithdrawingFunds;

namespace MyBank.API.Tests.UnitTests;

public class WithdrawFundsTests
{
    private List<BankAccount> _bankAccounts = new(){
        new(){
            Id = 1,
            Owner = "Tests",
            Balance = 10000,
        },
        new(){
            Id = 2,
            Owner = "Tests 2",
            Balance = 0,
        },
    };

    [Fact]
    public async Task ValidWithdrawAndBankAccountHasFunds_WithdrawTheAmount()
    {
        // Arrange
        var withdrawAmount = 1000;
        var bankAccount = _bankAccounts.First();
        var initialBalance = bankAccount.Balance;

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Equal(-withdrawAmount, tran.Amount);
            Assert.Equal(initialBalance, tran.OldBalance);
            Assert.Equal(bankAccount.Id, tran.BankAccountId);
            Assert.Equal(DateTime.Now.Date, tran.Date.Date);

            return ValueTask.FromResult(1);
        };

        var withdrawFundsRequest = new WithdrawFundsRequest(bankAccount.Id, withdrawAmount);

        // Act
        await WithdrawFunds.HandleAsync(withdrawFundsRequest, _bankAccounts.AsQueryable(), addAndSaveAsyncAssert);

        // Assert
        Assert.Equal(initialBalance - withdrawAmount, bankAccount.Balance);
    }

    [Fact]
    public void ValidWithdrawAndBanAccountWithoutFunds_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var withdrawAmount = 1000;
        var bankAccount = _bankAccounts[1];

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Fail("AddAndSave should not be called.");
            return ValueTask.FromResult(1);
        };

        var withdrawFundsRequest = new WithdrawFundsRequest(bankAccount.Id, withdrawAmount);

        // Act
        _ = Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            async () => await WithdrawFunds.HandleAsync(
                withdrawFundsRequest,
                _bankAccounts.AsQueryable(),
                addAndSaveAsyncAssert
            )
        );

        // Assert
        Assert.Equal(0, bankAccount.Balance);
    }

    [Fact]
    public void WithdrawRequestWithInvalidBankAccountId_ShouldTrowsException()
    {
        // Arrange
        var withdrawAmount = 1000;
        var invalidBankAccountId = 10;
        var bankAccount = _bankAccounts.First();
        var initialBalance = bankAccount.Balance;

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Fail("AddAndSave should not be called.");
            return ValueTask.FromResult(1);
        };

        var withdrawFundsRequest = new WithdrawFundsRequest(invalidBankAccountId, withdrawAmount);

        // Act
        _ = Assert.ThrowsAsync<Exception>(
            async () => await WithdrawFunds.HandleAsync(withdrawFundsRequest, _bankAccounts.AsQueryable(), addAndSaveAsyncAssert)
        );

        // Assert
        Assert.Equal(initialBalance, bankAccount.Balance);
    }

    [Fact]
    public void WithdrawRequestWithNegativeAmount_ShouldThrowsException()
    {
        // Arrange
        var withdrawAmount = -1000;
        var bankAccount = _bankAccounts.First();
        var initialBalance = bankAccount.Balance;

        var addAndSaveAsyncAssert = (BankTransaction tran) =>
        {
            Assert.Fail("AddAndSave should not be called.");
            return ValueTask.FromResult(1);
        };

        var withdrawFundsRequest = new WithdrawFundsRequest(bankAccount.Id, withdrawAmount);

        // Act
        _ = Assert.ThrowsAsync<Exception>(
            async () => await WithdrawFunds.HandleAsync(
                withdrawFundsRequest,
                _bankAccounts.AsQueryable(),
                addAndSaveAsyncAssert
            )
        );

        // Assert
        Assert.Equal(initialBalance, bankAccount.Balance);
    }
}