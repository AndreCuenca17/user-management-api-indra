using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Authorization;
using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Ports.Input;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IListUsersUseCase _listUsers;
    private readonly IUpdateUserUseCase _updateUser;

    public UsersController(IListUsersUseCase listUsers, IUpdateUserUseCase updateUser)
    {
        _listUsers = listUsers;
        _updateUser = updateUser;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<UserDto>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _listUsers.ExecuteAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = PermissionPolicies.UsersUpdate)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> Update(
        int id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _updateUser.ExecuteAsync(id, request, cancellationToken);
        return Ok(user);
    }
}
