using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.OpeningBankAccount;

public class Endpoint : EndpointBaseAsync.WithRequest<OpenBankAccountRequest>.WithActionResult
{
    private readonly MyBankDbContext _myBankDbContext;

    public Endpoint(MyBankDbContext myBankDbContext)
    {
        _myBankDbContext = myBankDbContext;
    }

    [HttpPost("/api/open-bank-account")]
    [SwaggerOperation(Tags = new string[] { "BankAccount" })]
    public override async Task<ActionResult> HandleAsync(OpenBankAccountRequest request, CancellationToken cancellationToken = default)
    {
        OpenBankAccount.HandleAsync(
            request,
            _myBankDbContext.AddAndSaveChangesAsync
        );

        return Accepted();
    }
}
