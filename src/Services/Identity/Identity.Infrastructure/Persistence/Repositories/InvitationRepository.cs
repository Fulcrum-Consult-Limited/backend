namespace Identity.Infrastructure.Persistence.Repositories;

public sealed class InvitationRepository(AppDbContext context) : IInvitationRepository
{
    public async Task<Invitation?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Invitations.FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<Invitation?> GetByTokenAsync(string token, CancellationToken ct = default) =>
        await context.Invitations.FirstOrDefaultAsync(i => i.Token == token, ct);

    public async Task<Invitation?> GetPendingByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        await context.Invitations.FirstOrDefaultAsync(
            i => i.UserId == userId && !i.IsUsed, ct);

    public async Task AddAsync(Invitation invitation, CancellationToken ct = default) =>
        await context.Invitations.AddAsync(invitation, ct);

    public async Task UpdateAsync(Invitation invitation, CancellationToken ct = default) =>
        context.Invitations.Update(invitation);

    public async Task<(IReadOnlyList<Invitation> Invitations, int TotalCount)> ListPendingAsync(
        int page,
        int pageSize,
        bool includeExpired = false,
        CancellationToken ct = default)
    {
        var query = context.Invitations
            .Where(i => !i.IsUsed);

        if (!includeExpired)
            query = query.Where(i => i.ExpiresAt > DateTime.UtcNow);

        var totalCount = await query.CountAsync(ct);

        var invitations = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (invitations, totalCount);
    }
}