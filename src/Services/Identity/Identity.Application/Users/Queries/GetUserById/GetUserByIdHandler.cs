namespace Identity.Application.Users.Queries.GetUserById;

public sealed class GetUserByIdHandler(IUserRepository userRepository)
{
    public async Task<Result<UserDTO>> Handle(GetUserByIdQuery query, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(query.UserId, ct);

        if (user is null)
            return Result<UserDTO>.Failure(IdentityErrors.UserNotFound);

        return Result<UserDTO>.Success(user.ToDto());
    }
}
