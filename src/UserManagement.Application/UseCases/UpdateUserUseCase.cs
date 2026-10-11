using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Ports.Input;
using UserManagement.Application.Ports.Output;
using UserManagement.Domain.Exceptions;

namespace UserManagement.Application.UseCases;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> ExecuteAsync(int id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
                   ?? throw new UserNotFoundException(id);

        user.Update(request.Name, request.Email, request.IsActive);

        if (await _userRepository.EmailExistsAsync(user.Email, id, cancellationToken))
            throw new EmailAlreadyInUseException(user.Email);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return UserDto.FromEntity(user);
    }
}
