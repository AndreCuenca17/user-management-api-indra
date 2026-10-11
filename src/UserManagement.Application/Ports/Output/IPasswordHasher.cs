namespace UserManagement.Application.Ports.Output;

public interface IPasswordHasher
{
    bool Verify(string password, string passwordHash);
}
