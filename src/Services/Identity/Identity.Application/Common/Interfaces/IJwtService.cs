using Identity.Domain.Entities;

namespace Identity.Application.Common.Interfaces;

public interface IJwtService
{
    /// <summary>
    /// Generates a signed JWT for the given user.
    /// Returns the token string and the jti (JWT ID) used for revocation.
    /// </summary>
    (string Token, string Jti, DateTime ExpiresAt) Generate(User user);

    /// <summary>
    /// Extracts the jti and expiry from a token without validating the signature.
    /// Used during logout to revoke a token that has already been verified by the middleware.
    /// </summary>
    (string Jti, DateTime ExpiresAt) ExtractClaims(string token);
}