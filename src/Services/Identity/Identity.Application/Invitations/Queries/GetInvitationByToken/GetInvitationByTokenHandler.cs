namespace Identity.Application.Invitations.Queries.GetInvitationByToken;

public sealed class GetInvitationByTokenHandler(IInvitationRepository invitationRepository)
{
    public async Task<Result<InvitationDTO>> Handle(
        GetInvitationByTokenQuery query,
        CancellationToken ct = default)
    {
        var invitation = await invitationRepository.GetByTokenAsync(query.Token, ct);

        if (invitation is null)
            return Result<InvitationDTO>.Failure(IdentityErrors.InvitationNotFound);

        if (invitation.IsExpired())
            return Result<InvitationDTO>.Failure(IdentityErrors.InvitationExpired);

        if (invitation.IsUsed)
            return Result<InvitationDTO>.Failure(IdentityErrors.InvitationAlreadyUsed);

        return Result<InvitationDTO>.Success(invitation.ToDto());
    }
}