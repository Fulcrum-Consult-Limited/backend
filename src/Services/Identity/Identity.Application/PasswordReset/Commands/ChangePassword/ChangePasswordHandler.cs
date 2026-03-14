namespace Identity.Application.PasswordReset.Commands.ChangePassword;

public sealed class ChangePasswordHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(
        ChangePasswordCommand command,
        CancellationToken ct = default)
    {
        Password newPassword;
        try
        {
            newPassword = Password.Create(command.NewPassword);
        }
        catch (WeakPasswordException ex)
        {
            return Result.Failure(IdentityErrors.WeakPassword with
            {
                Message = ex.Message
            });
        }

        var user = await userRepository.GetByIdAsync(command.UserId, ct);

        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        if (!user.IsActive)
            return Result.Failure(IdentityErrors.UserInactive);

        if (!passwordHasher.Verify(command.CurrentPassword, user.PasswordHash))
            return Result.Failure(IdentityErrors.InvalidCredentials);

        user.ChangePassword(passwordHasher.Hash(newPassword.Value));
        await userRepository.UpdateAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        return Result.Success();
    }
}