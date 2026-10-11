using UserManagement.Application.DTOs.Auth;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Ports.Input;
using UserManagement.Application.Ports.Output;

namespace UserManagement.Application.UseCases;

public class AuthenticateUserUseCase : IAuthenticateUserUseCase
{
    private const string TokenType = "Bearer";

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthenticateUserUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<TokenResponse> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        if (!user.IsActive)
            throw new InvalidCredentialsException("El usuario está inactivo.");

        var token = _tokenGenerator.Generate(user);
        return new TokenResponse(token.AccessToken, TokenType, token.ExpiresInSeconds);
    }
}
