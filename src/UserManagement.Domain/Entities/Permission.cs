namespace UserManagement.Domain.Entities;

public class Permission
{
    public int Id { get; private set; }
    public string Name { get; private set; } =  string.Empty;
    
    private Permission(){}

    public Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }
}