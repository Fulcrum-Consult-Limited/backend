using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces;

public interface IPasswordResetTokenRepository
{
    Task<PasswordResetToken?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken ct = default);

    Task InvalidateAllForUserAsync(Guid userId, CancellationToken ct = default);

    Task AddAsync(PasswordResetToken token, CancellationToken ct = default);
    Task UpdateAsync(PasswordResetToken token, CancellationToken ct = default);
}