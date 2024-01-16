using MyBank.API.Domain;

namespace MyBank.API.Features.SigningUp;

public record CreateUserRequest(string UserName, string Password);

internal static class OpeningUserAccount
{
    internal static async Task HandleAsync(CreateUserRequest request, Func<User, ValueTask<int>> addAndSaveAsync)
    {
        //TODO: Validate user name already taken 
        var user = new User
        {
            UserName = request.UserName,
        };

        user.PasswordHash = new PasswordHasher().HashPassword(user, request.Password);

        await addAndSaveAsync(user);
    }
}