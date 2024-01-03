using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyBank.API.Data;
using MyBank.API.Features.OpeningBankAccount;
using System.Net;
using System.Net.Http.Json;

namespace MyBank.API.Tests.IntegrationTests;

public class OpenBankAccountEndpointTests
{
    private object application;

    [Fact]
    public async Task OpenBnakAccount_RequestValid_ShouldCreateBankAccount()
    {
        // Arrange
        await using var application = new MyBankApplication();
        var client = application.CreateClient();

        var payload = new OpenBankAccountRequest("Test Account");

        // Act
        var response = await client.PostAsJsonAsync("/api/open-bank-account", payload);

        // Assert
        response.EnsureSuccessStatusCode();

        using var scope = application.Services.CreateScope();
        var bankAccounts = scope.ServiceProvider.GetRequiredService<MyBankDbContext>().BankAccounts;

        bankAccounts.Count().Should().Be(1);
        bankAccounts.First().Owner.Should().Be("Test Account");
        bankAccounts.First().Balance.Should().Be(0);
    }

    [Fact]
    public async Task OpenBnakAccount_InvalidRequest_ShouldRespondWithBadRequest()
    {
        // Arrange
        await using var application = new MyBankApplication();

        HttpClient client = application.CreateClient();

        var payload = new OpenBankAccountRequest("");

        // Act
        var response = await client.PostAsJsonAsync("/api/open-bank-account", payload);

        using var scope = application.Services.CreateScope();
        var bankAccounts = scope.ServiceProvider.GetRequiredService<MyBankDbContext>().BankAccounts;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        bankAccounts.Count().Should().Be(0);
    }

    [Fact]
    public async Task OpenBnakAccount_RequestValidButPersistenceFailed_ShouldRespondWithBadRequest()
    {
        // Arrange
        await using var application = new MyBankApplication()
            .WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(srv =>
                    srv.RemoveAll<MyBankDbContext>()
                        .AddDbContext<MyBankDbContext, MyBankDbContextFailure>(opts => opts.UseInMemoryDatabase("Testing")))
        );

        var client = application.CreateClient();

        var payload = new OpenBankAccountRequest("Test Account");

        // Act
        var response = await client.PostAsJsonAsync("/api/open-bank-account", payload);

        using var scope = application.Services.CreateScope();
        var bankAccounts = scope.ServiceProvider.GetRequiredService<MyBankDbContext>().BankAccounts;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        bankAccounts.Count().Should().Be(0);
    }
}
