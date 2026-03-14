namespace Identity.Application.Invitations.Commands.ResendInvitation;

public sealed class ResendInvitationHandler(
    IUserRepository userRepository,
    IInvitationRepository invitationRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<InvitationDTO>> Handle(
        ResendInvitationCommand command,
        CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, ct);

        if (user is null)
            return Result<InvitationDTO>.Failure(IdentityErrors.UserNotFound);

        if (user.IsActive)
            return Result<InvitationDTO>.Failure(IdentityErrors.UserAlreadyRegistered);

        var existing = await invitationRepository.GetPendingByUserIdAsync(command.UserId, ct);
        if (existing is not null)
        {
            existing.Invalidate();
            await invitationRepository.UpdateAsync(existing, ct);
        }

        var invitation = Invitation.Create(command.UserId, user.Email);
        await invitationRepository.AddAsync(invitation, ct);
        await unitOfWork.CommitAsync(ct);

        return Result<InvitationDTO>.Success(invitation.ToDto());
    }
}