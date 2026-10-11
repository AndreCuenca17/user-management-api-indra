using UserManagement.Domain.Entities;

namespace UserManagement.Application.Ports.Output;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task<bool> EmailExistsAsync(string email, int excludeUserId, CancellationToken cancellationToken);

    Task<(IReadOnlyList<User> Items , int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

    Task UpdateAsync(User user, CancellationToken cancellationToken);
}
