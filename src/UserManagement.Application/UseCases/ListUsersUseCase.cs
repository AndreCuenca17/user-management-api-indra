using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Ports.Input;
using UserManagement.Application.Ports.Output;
using UserManagement.Domain.Exceptions;

namespace UserManagement.Application.UseCases;

public class ListUsersUseCase : IListUsersUseCase
{
    public const int MaxPageSize = 100;

    private readonly IUserRepository _userRepository;

    public ListUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult<UserDto>> ExecuteAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        if (page < 1)
            throw new DomainValidationException("El parámetro page debe ser mayor o igual a 1.");

        if (pageSize < 1 || pageSize > MaxPageSize)
            throw new DomainValidationException($"El parámetro pageSize debe estar entre 1 y {MaxPageSize}.");

        var (users, totalCount) = await _userRepository.GetPagedAsync(page, pageSize, cancellationToken);
        var items = users.Select(UserDto.FromEntity).ToList();

        return new PagedResult<UserDto>(items, page, pageSize, totalCount);
    }
}
