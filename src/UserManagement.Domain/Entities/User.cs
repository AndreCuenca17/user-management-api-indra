using System.Text.RegularExpressions;
using UserManagement.Domain.Exceptions;

namespace UserManagement.Domain.Entities;

public class User
{
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public ICollection<Role> Roles { get; private set; } = new List<Role>();

    private User() { }

    public User(int id, string name, string email, string passwordHash, bool isActive)
    {
        Id = id;
        Name = ValidateName(name);
        Email = ValidateEmail(email);
        PasswordHash = passwordHash;
        IsActive = isActive;
    }

    public void Update(string name, string email, bool isActive)
    {
        var validName = ValidateName(name);
        var validEmail = ValidateEmail(email);

        Name = validName;
        Email = validEmail;
        IsActive = isActive;
    }

    public IReadOnlyCollection<string> GetPermissions() =>
        Roles.SelectMany(role => role.Permissions)
             .Select(permission => permission.Name)
             .Distinct()
             .ToList();

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainValidationException("El nombre es obligatorio.");

        return name.Trim();
    }

    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !EmailPattern.IsMatch(email))
            throw new DomainValidationException("El formato del email no es válido.");

        return email.Trim().ToLowerInvariant();
    }
}
