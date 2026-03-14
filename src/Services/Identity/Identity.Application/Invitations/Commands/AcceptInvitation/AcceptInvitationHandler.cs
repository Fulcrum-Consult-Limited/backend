namespace Identity.Application.Invitations.Commands.AcceptInvitation;

public sealed class AcceptInvitationHandler(
    IInvitationRepository invitationRepository,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(
        AcceptInvitationCommand command,
        CancellationToken ct = default)
    {
        Password password;
        try
        {
            password = Password.Create(command.Password);
        }
        catch (WeakPasswordException ex)
        {
            return Result.Failure(IdentityErrors.WeakPassword with
            {
                Message = ex.Message
            });
        }

        var invitation = await invitationRepository.GetByTokenAsync(command.Token, ct);

        if (invitation is null)
            return Result.Failure(IdentityErrors.InvitationNotFound);

        try
        {
            invitation.Use();
        }
        catch (InvitationExpiredException)
        {
            return Result.Failure(IdentityErrors.InvitationExpired);
        }
        catch (InvitationAlreadyUsedException)
        {
            return Result.Failure(IdentityErrors.InvitationAlreadyUsed);
        }

        var user = await userRepository.GetByIdAsync(invitation.UserId, ct);

        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        var hash = passwordHasher.Hash(password.Value);
        user.CompleteRegistration(command.Forename, command.Surname, hash);

        await userRepository.UpdateAsync(user, ct);
        await invitationRepository.UpdateAsync(invitation, ct);
        await unitOfWork.CommitAsync(ct);

        return Result.Success();
    }
}