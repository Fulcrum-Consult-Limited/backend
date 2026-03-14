namespace Identity.Domain.Exceptions;

public sealed class InvitationAlreadyUsedException : DomainException
{
    public InvitationAlreadyUsedException(Guid invitationId)
        : base($"Invitation '{invitationId}' has already been used.") { }
}