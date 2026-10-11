namespace UserManagement.Application.DTOs.Auth;

public record TokenResponse(string AccessToken, string TokenType, int ExpiresIn);
