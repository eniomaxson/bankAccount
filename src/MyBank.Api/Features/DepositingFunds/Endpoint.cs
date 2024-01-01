using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.DepositingFunds;

public class Endpoint : EndpointBaseAsync.WithRequest<DepositFundsRequest>.WithActionResult
{
    private readonly MyBankDbContext _myBankDbContext;

    public Endpoint(MyBankDbContext myBankDbContext)
    {
        _myBankDbContext = myBankDbContext;
    }

    [HttpPost("/api/deposit-funds")]
    [SwaggerOperation(Tags = new string[] { "BankAccount" })]
    public override async Task<ActionResult> HandleAsync(DepositFundsRequest request, CancellationToken cancellationToken = default)
    {
        await DepositFunds.HandleAsync(
            request,
            _myBankDbContext.BankAccounts,
            _myBankDbContext.AddAndSaveChangesAsync
        );

        return Accepted();
    }
}
