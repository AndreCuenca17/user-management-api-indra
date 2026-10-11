using UserManagement.Domain.Entities;

namespace UserManagement.Application.DTOs.Users;

public record UserDto(int Id, string Name, string Email, bool IsActive)
{
    public static UserDto FromEntity(User user) => new(user.Id, user.Name, user.Email, user.IsActive);
}
