using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.WithdrawingFunds;

public class Endpoint : EndpointBaseAsync.WithRequest<WithdrawFundsRequest>.WithActionResult
{
    private readonly MyBankDbContext _myBankDbContext;

    public Endpoint(MyBankDbContext myBankDbContext)
    {
        _myBankDbContext = myBankDbContext;
    }

    [HttpPost("/api/withdraw-funds")]
    [SwaggerOperation(Tags = new string[] { "BankAccount" })]
    public override async Task<ActionResult> HandleAsync(WithdrawFundsRequest request, CancellationToken cancellationToken = default)
    {
        await WithdrawFunds.HandleAsync(
            request,
            _myBankDbContext.BankAccounts,
            _myBankDbContext.AddAndSaveChangesAsync
        );

        return Accepted();
    }
}
