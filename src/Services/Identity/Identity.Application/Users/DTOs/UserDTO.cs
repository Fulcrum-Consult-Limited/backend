namespace Identity.Application.Users.DTOs;

public sealed record UserDTO(
    Guid Id,
    string Email,
    string Forename,
    string Surname,
    string FullName,
    string Role,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? LastLoginAt);