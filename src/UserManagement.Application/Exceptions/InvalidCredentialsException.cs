namespace UserManagement.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message = "Credenciales incorrectas.") : base(message)
    {
    }
}
