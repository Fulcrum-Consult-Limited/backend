namespace Identity.Infrastructure.Persistence.Repositories;

public sealed class PasswordResetTokenRepository(AppDbContext context) : IPasswordResetTokenRepository
{
    public async Task<PasswordResetToken?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.PasswordResetTokens.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken ct = default) =>
        await context.PasswordResetTokens.FirstOrDefaultAsync(p => p.Token == token, ct);

    public async Task InvalidateAllForUserAsync(Guid userId, CancellationToken ct = default) =>
        await context.PasswordResetTokens
            .Where(p => p.UserId == userId && !p.IsUsed)
            .ExecuteUpdateAsync(
                s => s.SetProperty(p => p.ExpiresAt, DateTime.UtcNow),
                ct);

    public async Task AddAsync(PasswordResetToken token, CancellationToken ct = default) =>
        await context.PasswordResetTokens.AddAsync(token, ct);

    public async Task UpdateAsync(PasswordResetToken token, CancellationToken ct = default) =>
        context.PasswordResetTokens.Update(token);
}