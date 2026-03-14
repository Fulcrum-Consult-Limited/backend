namespace Identity.Domain.Interfaces;

/// <summary>
/// Contract for revoking and checking JWT access tokens.
/// Implementation lives in Infrastructure (Redis).
/// The domain owns the contract because revocation is a business rule —
/// a revoked user must not be able to authenticate regardless of token validity.
/// </summary>
public interface ITokenRevocationStore
{
    /// <summary>
    /// Marks a JWT as revoked. TTL should match the token's remaining lifetime
    /// so the store doesn't grow unboundedly.
    /// </summary>
    Task RevokeAsync(string jti, TimeSpan ttl, CancellationToken ct = default);

    /// <summary>
    /// Returns true if the token has been explicitly revoked.
    /// Called on every authenticated request via middleware.
    /// </summary>
    Task<bool> IsRevokedAsync(string jti, CancellationToken ct = default);
}