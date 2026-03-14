namespace Identity.Domain.Exceptions;

public sealed class InvitationExpiredException : DomainException
{
    public InvitationExpiredException(Guid invitationId)
        : base($"Invitation '{invitationId}' has expired.") { }
}