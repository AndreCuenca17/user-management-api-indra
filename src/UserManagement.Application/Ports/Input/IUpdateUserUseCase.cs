using UserManagement.Application.DTOs.Users;

namespace UserManagement.Application.Ports.Input;

public interface IUpdateUserUseCase
{
    Task<UserDto> ExecuteAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken);
}
