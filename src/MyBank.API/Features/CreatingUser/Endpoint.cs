using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.SigningUp
{
    [Route("api/v1")]
    public class Endpoint : EndpointBaseAsync.WithRequest<CreateUserRequest>.WithActionResult
    {
        private readonly MyBankDbContext _dbContext;

        public Endpoint(MyBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("signup")]
        [SwaggerOperation(Tags = new string[] { "Accounts" })]
        public override async Task<ActionResult> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            await OpeningUserAccount.HandleAsync(request, _dbContext.AddAndSaveChangesAsync);

            return Accepted();
        }
    }
}
