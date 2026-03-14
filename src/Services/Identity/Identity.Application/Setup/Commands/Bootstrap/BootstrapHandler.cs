
namespace Identity.Application.Setup.Commands.Bootstrap;

public sealed class BootstrapHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<UserDTO>> Handle(
        BootstrapCommand command,
        CancellationToken ct = default)
    {
        var anyUsers = await userRepository.AnyAsync(ct);
        if (anyUsers)
            return Result<UserDTO>.Failure(IdentityErrors.BootstrapAlreadyCompleted);

        Email email;
        try
        {
            email = Email.Create(command.Email);
        }
        catch (InvalidEmailException)
        {
            return Result<UserDTO>.Failure(IdentityErrors.InvalidEmail);
        }

        Password password;
        try
        {
            password = Password.Create(command.Password);
        }
        catch (WeakPasswordException ex)
        {
            return Result<UserDTO>.Failure(IdentityErrors.WeakPassword with
            {
                Message = ex.Message
            });
        }

        var user = User.CreateFirstAdmin(
            email,
            command.Forename,
            command.Surname,
            passwordHasher.Hash(password.Value));

        await userRepository.AddAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        return Result<UserDTO>.Success(user.ToDto());
    }
}