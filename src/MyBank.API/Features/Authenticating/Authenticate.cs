using MyBank.API.Domain;
using System.Text.Json.Serialization;

namespace MyBank.API;

public record AuthenticateRequest(string UserName, string Password);

public class AuthenticateResponse
{
    [JsonIgnore]
    public bool IsAuthenticated { get; } = false;
    public string? AccessToken { get; }
    public string TokenType { get; } = "Bearer";

    private AuthenticateResponse(string accessToken)
    {
        AccessToken = accessToken;
        IsAuthenticated = true;
    }

    private AuthenticateResponse()
    { }

    public static AuthenticateResponse SuccessResponse(string token) => new(token);
    public static AuthenticateResponse FailResponse() => new();
}

internal static class Authenticate
{
    internal static AuthenticateResponse Handle(AuthenticateRequest request, IQueryable<User> users, IConfiguration configuration)
    {
        var user = users.FirstOrDefault(x => x.UserName == request.UserName);
        if (user is null)
        {
            return AuthenticateResponse.FailResponse();
        }
        if (!new PasswordHasher().VerifyHashedPassword(user, request.Password))
        {
            return AuthenticateResponse.FailResponse();
        }
        var tokenGenerator = new JwtSecurityTokenGenerator(configuration);
        return AuthenticateResponse.SuccessResponse(tokenGenerator.GenerateToken(user));
    }
}
