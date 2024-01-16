using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using MyBank.API.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBank.API.Features.Authenticating
{
    [Route("api/v1")]
    public class Endpoint : EndpointBaseAsync.WithRequest<AuthenticateRequest>.WithActionResult
    {
        private readonly IConfiguration _configuration;
        private readonly MyBankDbContext _dbContext;

        public Endpoint(IConfiguration configuration, MyBankDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("authenticate")]
        [SwaggerOperation(Tags = new string[] { "Accounts" })]
        public override async Task<ActionResult> HandleAsync(AuthenticateRequest request, CancellationToken cancellationToken = default)
        {
            var authenticateResponse = Authenticate.Handle(request, _dbContext.Users, _configuration);
            return authenticateResponse.IsAuthenticated ? Ok(authenticateResponse) : BadRequest();
        }
    }
}
