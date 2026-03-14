namespace Identity.Application.Users.Commands.DeactivateUser;

public sealed class DeactivateUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(DeactivateUserCommand command, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, ct);

        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        if (!user.IsActive)
            return Result.Success();

        user.Deactivate();
        await userRepository.UpdateAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        return Result.Success();
    }
}