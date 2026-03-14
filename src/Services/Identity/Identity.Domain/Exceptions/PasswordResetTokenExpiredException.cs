namespace Identity.Domain.Exceptions;

public sealed class PasswordResetTokenExpiredException : DomainException
{
    public PasswordResetTokenExpiredException(Guid tokenId)
        : base($"Password reset token '{tokenId}' has expired.") { }
}
