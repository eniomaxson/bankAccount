using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.BankStatement
{
    public class Endpoint : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly MyBankDbContext _myBankDbContext;

        public Endpoint(MyBankDbContext myBankDbContext)
        {
            _myBankDbContext = myBankDbContext;
        }

        [HttpGet("api/bank-statement")]
        [SwaggerOperation(Tags = new string[] { "BankAccount" })]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await _myBankDbContext.BankAccounts.Include(x => x.Transactions).ToListAsync());
        }
    }
}
