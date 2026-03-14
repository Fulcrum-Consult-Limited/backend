namespace Identity.Domain.Interfaces;

public interface IInvitationRepository
{
    Task<Invitation?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Invitation?> GetByTokenAsync(string token, CancellationToken ct = default);
    Task<Invitation?> GetPendingByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Invitation invitation, CancellationToken ct = default);
    Task UpdateAsync(Invitation invitation, CancellationToken ct = default);

    Task<(IReadOnlyList<Invitation> Invitations, int TotalCount)> ListPendingAsync(
        int page,
        int pageSize,
        bool includeExpired = false,
        CancellationToken ct = default);
}