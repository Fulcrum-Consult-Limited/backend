namespace Identity.Application.Invitations.Commands.CreateInvitation;

public sealed class CreateInvitationHandler(
    IUserRepository userRepository,
    IInvitationRepository invitationRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<InvitationDTO>> Handle(
        CreateInvitationCommand command,
        CancellationToken ct = default)
    {
        Email email;
        try
        {
            email = Email.Create(command.Email);
        }
        catch (InvalidEmailException)
        {
            return Result<InvitationDTO>.Failure(IdentityErrors.InvalidEmail);
        }

        if (await userRepository.ExistsByEmailAsync(email, ct))
            return Result<InvitationDTO>.Failure(IdentityErrors.UserAlreadyExists);

        var user = User.CreatePending(email, command.Role);
        await userRepository.AddAsync(user, ct);

        var existing = await invitationRepository.GetPendingByUserIdAsync(user.Id, ct);
        if (existing is not null && !existing.IsExpired())
            return Result<InvitationDTO>.Failure(IdentityErrors.PendingInvitationExists);

        var invitation = Invitation.Create(user.Id, email);
        await invitationRepository.AddAsync(invitation, ct);
        await unitOfWork.CommitAsync(ct);

        return Result<InvitationDTO>.Success(invitation.ToDto());
    }
}