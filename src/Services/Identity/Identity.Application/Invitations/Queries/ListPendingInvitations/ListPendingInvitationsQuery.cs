namespace Identity.Application.Invitations.Queries.ListPendingInvitations;

public sealed record ListPendingInvitationsQuery(
    int Page = 1,
    int PageSize = 20,
    bool IncludeExpired = false);
