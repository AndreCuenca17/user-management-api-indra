using UserManagement.Application.Ports.Output;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Tests.Fakes;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users;

    public FakeUserRepository(params User[] users)
    {
        _users = users.ToList();
    }

    public int UpdateCalls { get; private set; }

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        Task.FromResult(_users.FirstOrDefault(user => user.Id == id));

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        Task.FromResult(_users.FirstOrDefault(user => user.Email == email));

    public Task<bool> EmailExistsAsync(string email, int excludeUserId, CancellationToken cancellationToken) =>
        Task.FromResult(_users.Any(user => user.Email == email && user.Id != excludeUserId));

    public Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, CancellationToken cancellationToken)
    {
        IReadOnlyList<User> items = _users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return Task.FromResult((items, _users.Count));
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        UpdateCalls++;
        return Task.CompletedTask;
    }
}
