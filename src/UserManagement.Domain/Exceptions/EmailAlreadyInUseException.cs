namespace UserManagement.Domain.Exceptions;

public class EmailAlreadyInUseException : DomainException
{
    public EmailAlreadyInUseException(string email) : base($"El email '{email}' ya pertenece a otro usuario.")
    {
    }
}
