namespace Identity.Application.Invitations.Commands.CreateInvitation;

public sealed record CreateInvitationCommand(
    string Email,
    UserRole Role = UserRole.User);