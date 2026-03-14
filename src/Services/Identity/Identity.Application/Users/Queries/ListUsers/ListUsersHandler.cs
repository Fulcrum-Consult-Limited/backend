namespace Identity.Application.Users.Queries.ListUsers;

public sealed class ListUsersHandler(IUserRepository userRepository)
{
    public async Task<Result<PagedResult<UserDTO>>> Handle(
        ListUsersQuery query,
        CancellationToken ct = default)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var (users, totalCount) = await userRepository.ListAsync(
            page,
            pageSize,
            query.Role,
            query.IsActive,
            query.SearchTerm,
            ct);

        var result = new PagedResult<UserDTO>(
            users.Select(u => u.ToDto()).ToList(),
            totalCount,
            page,
            pageSize);

        return Result<PagedResult<UserDTO>>.Success(result);
    }
}