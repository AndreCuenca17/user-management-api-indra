using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTOs.Auth;
using UserManagement.Application.Ports.Input;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticateUserUseCase _authenticateUser;

    public AuthController(IAuthenticateUserUseCase authenticateUser)
    {
        _authenticateUser = authenticateUser;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> CreateToken(LoginRequest request, CancellationToken cancellationToken)
    {
        var token = await _authenticateUser.ExecuteAsync(request, cancellationToken);
        return Ok(token);
    }
}
