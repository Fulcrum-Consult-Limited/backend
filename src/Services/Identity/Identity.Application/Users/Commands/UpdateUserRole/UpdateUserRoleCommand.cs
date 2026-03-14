namespace Identity.Application.Users.Commands.UpdateUserRole;

public sealed record UpdateUserRoleCommand(Guid UserId, UserRole NewRole);
