namespace Identity.Infrastructure.Persistence;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken ct = default) =>
        await context.SaveChangesAsync(ct);
}