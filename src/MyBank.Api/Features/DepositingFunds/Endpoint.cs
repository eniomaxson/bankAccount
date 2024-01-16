﻿using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.DepositingFunds;

[Authorize()]
[Route("api/v1")]
public class Endpoint : EndpointBaseAsync.WithRequest<DepositFundsRequest>.WithActionResult
{
    private readonly MyBankDbContext _myBankDbContext;
    private readonly ILogger<Endpoint> _logger;

    public Endpoint(MyBankDbContext myBankDbContext, ILogger<Endpoint> logger)
    {
        _myBankDbContext = myBankDbContext;
        _logger = logger;
    }

    [HttpPost("depositfunds")]
    [SwaggerOperation(Tags = new string[] { "BankAccount" })]
    public override async Task<ActionResult> HandleAsync(DepositFundsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await DepositFunds.HandleAsync(
                request,
                _myBankDbContext.BankAccounts,
                _myBankDbContext.AddAndSaveChangesAsync
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Deposit funds has failed!");

            return BadRequest(e.Message);
        }

        return Accepted();
    }
}
