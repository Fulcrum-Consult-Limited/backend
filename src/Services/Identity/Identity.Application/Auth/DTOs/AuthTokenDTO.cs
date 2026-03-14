namespace Identity.Application.Auth.DTOs;

public sealed record AuthTokenDTO(
    string AccessToken,
    DateTime ExpiresAt,
    string TokenType = "Bearer");