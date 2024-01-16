using System.Security.Cryptography;
using System.Text;
using MyBank.API.Domain;

namespace MyBank.API;

public class PasswordHasher
{
    public string HashPassword(User user, string password)
    {
        using var sha256 = SHA256.Create();
        var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + user.PasswordSalt));
        return Convert.ToHexString(computedHash);
    }

    public bool VerifyHashedPassword(User user, string providedPassword)
    {
        var hashProvidedPassword = HashPassword(user, providedPassword);
        return user.PasswordHash.Equals(hashProvidedPassword, StringComparison.OrdinalIgnoreCase);
    }
}
