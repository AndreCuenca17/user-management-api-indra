using BCrypt.Net;
using UserManagement.Application.Ports.Output;

namespace UserManagement.Infrastructure.Security;

public class BCryptPasswordHasher : IPasswordHasher
{
    public bool Verify(string password, string passwordHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch (SaltParseException)
        {
            return false;
        }
    }
}
