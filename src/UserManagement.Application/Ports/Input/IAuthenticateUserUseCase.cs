using UserManagement.Application.DTOs.Auth;

namespace UserManagement.Application.Ports.Input;

public interface IAuthenticateUserUseCase
{
    Task<TokenResponse> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken);
}
