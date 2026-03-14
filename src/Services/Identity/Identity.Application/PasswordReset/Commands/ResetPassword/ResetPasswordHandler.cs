namespace Identity.Application.PasswordReset.Commands.ResetPassword;

public sealed class ResetPasswordHandler(
    IPasswordResetTokenRepository tokenRepository,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(
        ResetPasswordCommand command,
        CancellationToken ct = default)
    {
        Password password;
        try
        {
            password = Password.Create(command.NewPassword);
        }
        catch (WeakPasswordException ex)
        {
            return Result.Failure(IdentityErrors.WeakPassword with
            {
                Message = ex.Message
            });
        }

        var token = await tokenRepository.GetByTokenAsync(command.Token, ct);

        if (token is null)
            return Result.Failure(IdentityErrors.PasswordResetTokenNotFound);

        try
        {
            token.Use();
        }
        catch (PasswordResetTokenExpiredException)
        {
            return Result.Failure(IdentityErrors.PasswordResetTokenExpired);
        }
        catch (PasswordResetTokenAlreadyUsedException)
        {
            return Result.Failure(IdentityErrors.PasswordResetTokenAlreadyUsed);
        }

        var user = await userRepository.GetByIdAsync(token.UserId, ct);

        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        if (!user.IsActive)
            return Result.Failure(IdentityErrors.UserInactive);

        user.ChangePassword(passwordHasher.Hash(password.Value));

        await userRepository.UpdateAsync(user, ct);
        await tokenRepository.UpdateAsync(token, ct);
        await unitOfWork.CommitAsync(ct);

        return Result.Success();
    }
}