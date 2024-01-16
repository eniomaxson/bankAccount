using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.GettingBankStatement;

[Authorize()]
[Route("api/v1")]
public class Endpoint : EndpointBaseAsync.WithoutRequest.WithActionResult
{
    private readonly MyBankDbContext _myBankDbContext;

    public Endpoint(MyBankDbContext myBankDbContext)
    {
        _myBankDbContext = myBankDbContext;
    }

    [HttpGet("bankstatement")]
    [SwaggerOperation(Tags = new string[] { "BankAccount" })]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        //TODO: Take only the transactions from the authenticated user
        //TODO: Add some filters here
        return Ok(await _myBankDbContext.BankAccounts.Include(x => x.Transactions).ToListAsync());
    }
}
