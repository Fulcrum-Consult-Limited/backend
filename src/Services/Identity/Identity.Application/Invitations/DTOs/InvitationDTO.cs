namespace Identity.Application.Invitations.DTOs;

public sealed record InvitationDTO(
    Guid Id,
    Guid UserId,
    string Email,
    DateTime ExpiresAt,
    bool IsUsed,
    DateTime? UsedAt,
    DateTime CreatedAt);