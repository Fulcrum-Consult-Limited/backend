namespace Identity.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken ct = default) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<bool> AnyAsync(CancellationToken ct = default) =>
        await context.Users.AnyAsync(ct);

    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct = default) =>
        await context.Users.AnyAsync(u => u.Email == email, ct);

    public async Task AddAsync(User user, CancellationToken ct = default) =>
        await context.Users.AddAsync(user, ct);

    public async Task UpdateAsync(User user, CancellationToken ct = default) =>
        context.Users.Update(user);

    public async Task<(IReadOnlyList<User> Users, int TotalCount)> ListAsync(
        int page,
        int pageSize,
        UserRole? role = null,
        bool? isActive = null,
        string? searchTerm = null,
        CancellationToken ct = default)
    {
        var query = context.Users.AsQueryable();

        if (role.HasValue)
            query = query.Where(u => u.Role == role.Value);

        if (isActive.HasValue)
            query = query.Where(u => u.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(u => u.Email.Value.StartsWith(searchTerm.ToLowerInvariant()));

        var totalCount = await query.CountAsync(ct);

        var users = await query
            .OrderBy(u => u.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (users, totalCount);
    }
}