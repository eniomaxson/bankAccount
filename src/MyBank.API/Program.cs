using MyBank.API;
using MyBank.Domain;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

ConcurrentDictionary<string, BankAccount> database = new();

app.MapGet("/", () =>
{
    return Results.Ok(database.Values);
});

app.MapPost("/open", (OpenAccountCommand command) =>
{
    var bankAccount = new BankAccount(command.Owner, command.initialDeposit);

    database.TryAdd(bankAccount.Number, bankAccount);

    return Results.Created($"/{bankAccount.Number}/detail", bankAccount);
});

app.MapGet("/{accountNumber}/detail", (string accountNumber) =>
{
    if (database.TryGetValue(accountNumber, out var bankAccount))
    {
        return Results.Ok(bankAccount);
    }

    return Results.NotFound();
});

app.MapPut("/{accountNumber}/deposit", (string accountNumber, DepositCommand command) =>
{
    if (database.TryGetValue(accountNumber, out var bankAccount))
    {
        bankAccount.Deposit(command.Ammount, command.Note);
        return Results.Accepted();
    }

    return Results.BadRequest();
});

app.MapPut("/{accountNumber}/withdrawl", (string accountNumber, DepositCommand command) =>
{
    if (database.TryGetValue(accountNumber, out var bankAccount))
    {
        bankAccount.Withdrawal(command.Ammount, command.Note);
        return Results.Accepted();
    }

    return Results.BadRequest();
});

app.Run();