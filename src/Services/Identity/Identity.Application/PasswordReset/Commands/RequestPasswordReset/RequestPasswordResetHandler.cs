namespace Identity.Application.PasswordReset.Commands.RequestPasswordReset;

public sealed class RequestPasswordResetHandler(
    IUserRepository userRepository,
    IPasswordResetTokenRepository tokenRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<PasswordResetTokenDTO>> Handle(
        RequestPasswordResetCommand command,
        CancellationToken ct = default)
    {
        Email email;
        try
        {
            email = Email.Create(command.Email);
        }
        catch (InvalidEmailException)
        {
            return Result<PasswordResetTokenDTO>.Success(null!);
        }

        var user = await userRepository.GetByEmailAsync(email, ct);

        if (user is null || !user.IsActive)
            return Result<PasswordResetTokenDTO>.Success(null!);

        await tokenRepository.InvalidateAllForUserAsync(user.Id, ct);

        var token = PasswordResetToken.Create(user.Id, email);
        await tokenRepository.AddAsync(token, ct);

        user.RaisePasswordResetRequestedEvent(token.Id);
        await userRepository.UpdateAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        return Result<PasswordResetTokenDTO>.Success(new PasswordResetTokenDTO(
            token.Id,
            token.UserId,
            token.Email.Value,
            token.ExpiresAt,
            token.CreatedAt));
    }
}