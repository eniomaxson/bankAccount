using MyBank.Domain;
using System.Collections.Concurrent;

namespace MyBank.API
{
    public static class MyBankEndpointsV1
    {
        public static ConcurrentDictionary<string, BankAccount> database = new();

        public static void MapMyBankEndpoints(this WebApplication app)
        {
            app.MapGet("", HandleAccountList);
            app.MapPost("/open", HandleOpenAccount);
            app.MapGet("/{accountNumber}/detail", HandleAccountDetail);
            app.MapPut("/{accountNumber}/deposit", HandleDeposit);
            app.MapPut("/{accountNumber}/withdraw", HandleWithdrawl);
        }

        private static IResult HandleAccountDetail(string accountNumber)
        {
            if (database.TryGetValue(accountNumber, out var bankAccount))
            {
                return Results.Ok(bankAccount);
            }

            return Results.NotFound();
        }

        private static IResult HandleAccountList()
        {
            return Results.Ok(database.Values);
        }

        private static IResult HandleDeposit(string accountNumber, DepositCommand command)
        {
            if (database.TryGetValue(accountNumber, out var bankAccount))
            {
                bankAccount.Deposit(command.Ammount, command.Note);
                return Results.Accepted();
            }

            return Results.BadRequest();
        }

        private static IResult HandleWithdrawl(string accountNumber, DepositCommand command)
        {
            if (database.TryGetValue(accountNumber, out var bankAccount))
            {
                bankAccount.Withdrawal(command.Ammount, command.Note);
                return Results.Accepted();
            }

            return Results.BadRequest();
        }

        private static IResult HandleOpenAccount(OpenAccountCommand command)
        {
            var bankAccount = new BankAccount(command.Owner, command.initialDeposit);

            database.TryAdd(bankAccount.Number, bankAccount);

            return Results.Created($"/{bankAccount.Number}/detail", bankAccount);
        }
    }
}
