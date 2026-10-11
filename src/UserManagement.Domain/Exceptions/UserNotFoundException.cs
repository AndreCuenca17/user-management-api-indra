namespace UserManagement.Domain.Exceptions;

public class UserNotFoundException : DomainException
{
    public UserNotFoundException(int userId) : base($"No se encontró el usuario con ID {userId}.")
    {
    }
}
