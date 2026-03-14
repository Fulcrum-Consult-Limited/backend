namespace Identity.Application.Auth.Commands.Login;

public sealed class LoginHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<AuthTokenDTO>> Handle(
        LoginCommand command,
        CancellationToken ct = default)
    {
        Email email;
        try
        {
            email = Email.Create(command.Email);
        }
        catch (InvalidEmailException)
        {
            return Result<AuthTokenDTO>.Failure(IdentityErrors.InvalidCredentials);
        }

        var user = await userRepository.GetByEmailAsync(email, ct);

        var hash = user?.PasswordHash ?? string.Empty;
        var passwordValid = passwordHasher.Verify(command.Password, hash);

        if (user is null || !passwordValid)
            return Result<AuthTokenDTO>.Failure(IdentityErrors.InvalidCredentials);

        if (!user.IsActive)
            return Result<AuthTokenDTO>.Failure(IdentityErrors.UserInactive);

        var (token, _, expiresAt) = jwtService.Generate(user);

        user.RecordLogin();
        await userRepository.UpdateAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        return Result<AuthTokenDTO>.Success(new AuthTokenDTO(token, expiresAt));
    }
}