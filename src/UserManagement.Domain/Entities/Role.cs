namespace UserManagement.Domain.Entities;

public class Role
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public ICollection<Permission> Permissions { get; private set; } = new List<Permission>();

    private Role() { }

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
