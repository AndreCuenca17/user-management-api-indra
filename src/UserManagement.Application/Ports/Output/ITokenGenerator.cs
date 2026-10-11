using UserManagement.Application.DTOs.Auth;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Ports.Output;

public interface ITokenGenerator
{
    GeneratedToken Generate(User user);
}
