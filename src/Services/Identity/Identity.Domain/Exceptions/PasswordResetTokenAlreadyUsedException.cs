namespace Identity.Domain.Exceptions;

public sealed class PasswordResetTokenAlreadyUsedException : DomainException
{
    public PasswordResetTokenAlreadyUsedException(Guid tokenId)
        : base($"Password reset token '{tokenId}' has already been used.") { }
}