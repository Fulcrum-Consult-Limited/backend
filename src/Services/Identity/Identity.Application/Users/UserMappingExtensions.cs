namespace Identity.Application.Users;

internal static class UserMappingExtensions
{
    internal static UserDTO ToDto(this User user) => new(
        user.Id,
        user.Email.Value,
        user.Forename,
        user.Surname,
        user.FullName,
        user.Role.ToString(),
        user.IsActive,
        user.CreatedAt,
        user.UpdatedAt,
        user.LastLoginAt
    );
}