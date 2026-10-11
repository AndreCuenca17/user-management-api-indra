using UserManagement.Application.DTOs.Users;

namespace UserManagement.Application.Ports.Input;

public interface IListUsersUseCase
{
    Task<PagedResult<UserDto>> ExecuteAsync(int page, int pageSize, CancellationToken cancellationToken);
}
