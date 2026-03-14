namespace Identity.Application.Invitations.Commands.AcceptInvitation;

public sealed record AcceptInvitationCommand(
    string Token,
    string Forename,
    string Surname,
    string Password);