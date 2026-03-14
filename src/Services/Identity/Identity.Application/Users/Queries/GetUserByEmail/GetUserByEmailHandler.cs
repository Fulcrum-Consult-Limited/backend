namespace Identity.Application.Users.Queries.GetUserByEmail;

public sealed class GetUserByEmailHandler(IUserRepository userRepository)
{
    public async Task<Result<UserDTO>> Handle(GetUserByEmailQuery query, CancellationToken ct = default)
    {
        Email email;

        try
        {
            email = Email.Create(query.Email);
        }
        catch (InvalidEmailException)
        {
            return Result<UserDTO>.Failure(IdentityErrors.InvalidEmail);
        }

        var user = await userRepository.GetByEmailAsync(email, ct);

        if (user is null)
            return Result<UserDTO>.Failure(IdentityErrors.UserNotFound);

        return Result<UserDTO>.Success(user.ToDto());
    }
}