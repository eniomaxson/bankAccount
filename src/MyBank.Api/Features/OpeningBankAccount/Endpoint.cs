using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.OpeningBankAccount;


[Authorize()]
[Route("api/v1")]
public class Endpoint : EndpointBaseAsync.WithoutRequest.WithActionResult
{
    private readonly MyBankDbContext _myBankDbContext;
    private readonly ILogger<Endpoint> _logger;

    public Endpoint(MyBankDbContext myBankDbContext, ILogger<Endpoint> logger)
    {
        _myBankDbContext = myBankDbContext;
        _logger = logger;
    }

    [HttpPost("openbankaccount")]
    [SwaggerOperation(Tags = new string[] { "BankAccount" })]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new OpenBankAccountRequest(User.Identity!.Name!);
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
