namespace Identity.Application.Users.Commands.ReactivateUser;

public sealed class ReactivateUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(ReactivateUserCommand command, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, ct);

        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        if (user.IsActive)
            return Result.Success();

        user.Reactivate();
        await userRepository.UpdateAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        return Result.Success();
    }
}