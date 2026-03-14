namespace Identity.Application.Invitations;

internal static class InvitationMappingExtensions
{
    internal static InvitationDTO ToDto(this Invitation invitation) => new(
        invitation.Id,
        invitation.UserId,
        invitation.Email.Value,
        invitation.ExpiresAt,
        invitation.IsUsed,
        invitation.UsedAt,
        invitation.CreatedAt
    );
}