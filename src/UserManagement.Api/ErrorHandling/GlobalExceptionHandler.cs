using Microsoft.AspNetCore.Diagnostics;
using UserManagement.Application.Exceptions;
using UserManagement.Domain.Exceptions;

namespace UserManagement.Api.ErrorHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            DomainValidationException => (StatusCodes.Status400BadRequest, "Datos de entrada inválidos"),
            InvalidCredentialsException => (StatusCodes.Status401Unauthorized, "No autorizado"),
            UserNotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            EmailAlreadyInUseException => (StatusCodes.Status409Conflict, "Conflicto"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        var isUnexpected = statusCode == StatusCodes.Status500InternalServerError;
        if (isUnexpected)
            _logger.LogError(exception, "Error no controlado al procesar la solicitud.");

        httpContext.Response.StatusCode = statusCode;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Status = statusCode,
                Title = title,
                Detail = isUnexpected ? "Ocurrió un error inesperado." : exception.Message
            }
        });
    }
}
