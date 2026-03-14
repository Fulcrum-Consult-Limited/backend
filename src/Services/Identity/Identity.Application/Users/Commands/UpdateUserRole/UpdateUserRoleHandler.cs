namespace Identity.Application.Users.Commands.UpdateUserRole;

public sealed class UpdateUserRoleHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(UpdateUserRoleCommand command, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, ct);

        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        if (!user.IsActive)
            return Result.Failure(IdentityErrors.UserInactive);

        user.UpdateRole(command.NewRole);
        await userRepository.UpdateAsync(user, ct);
        await unitOfWork.CommitAsync(ct);

        return Result.Success();
    }
}