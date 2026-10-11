using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Auth;

public record LoginRequest(
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    string Email,

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    string Password);
