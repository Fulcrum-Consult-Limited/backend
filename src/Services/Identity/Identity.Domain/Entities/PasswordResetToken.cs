namespace Identity.Domain.Entities;

public sealed class PasswordResetToken : BaseEntity
{
    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(15);

    public Guid UserId { get; private set; }
    public Email Email { get; private set; } = null!;
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime? UsedAt { get; private set; }

    private PasswordResetToken() { }

    public static PasswordResetToken Create(Guid userId, Email email, TimeSpan? expiry = null)
    {
        return new PasswordResetToken
        {
            UserId = userId,
            Email = email,
            Token = GenerateToken(),
            ExpiresAt = DateTime.UtcNow.Add(expiry ?? DefaultExpiry),
            IsUsed = false
        };
    }

    public void Use()
    {
        if (IsUsed)
            throw new PasswordResetTokenAlreadyUsedException(Id);

        if (DateTime.UtcNow > ExpiresAt)
            throw new PasswordResetTokenExpiredException(Id);

        IsUsed = true;
        UsedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    private static string GenerateToken() =>
        Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(64))
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
}