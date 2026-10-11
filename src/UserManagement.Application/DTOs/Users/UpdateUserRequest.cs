using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.DTOs.Users;

public record UpdateUserRequest(
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    string Name,

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    string Email,

    bool IsActive);
