namespace Identity.Application.PasswordReset.DTOs;

public sealed record PasswordResetTokenDTO(
    Guid Id,
    Guid UserId,
    string Email,
    DateTime ExpiresAt,
    DateTime CreatedAt);