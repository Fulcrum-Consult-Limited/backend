namespace Identity.Application.Invitations.Queries.ListPendingInvitations;

public sealed class ListPendingInvitationsHandler(IInvitationRepository invitationRepository)
{
    public async Task<Result<PagedResult<InvitationDTO>>> Handle(
        ListPendingInvitationsQuery query,
        CancellationToken ct = default)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var (invitations, totalCount) = await invitationRepository.ListPendingAsync(
            page,
            pageSize,
            query.IncludeExpired,
            ct);

        var result = new PagedResult<InvitationDTO>(
            invitations.Select(i => i.ToDto()).ToList(),
            totalCount,
            page,
            pageSize);

        return Result<PagedResult<InvitationDTO>>.Success(result);
    }
}