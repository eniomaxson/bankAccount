using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.OpeningBankAccount;

public class Endpoint : EndpointBaseAsync.WithRequest<OpenBankAccountRequest>.WithActionResult
{
    private readonly MyBankDbContext _myBankDbContext;
    private readonly ILogger<Endpoint> _logger;

    public Endpoint(MyBankDbContext myBankDbContext, ILogger<Endpoint> logger)
    {
        _myBankDbContext = myBankDbContext;
        _logger = logger;
    }

    [HttpPost("/api/open-bank-account")]
    [SwaggerOperation(Tags = new string[] { "BankAccount" })]
    public override async Task<ActionResult> HandleAsync(OpenBankAccountRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await OpenBankAccount.HandleAsync(request, _myBankDbContext.AddAndSaveChangesAsync);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Open Account has failed");

            return BadRequest(e.Message);
        }

        return Accepted();
    }
}
