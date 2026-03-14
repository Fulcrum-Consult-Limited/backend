namespace Identity.Application.Users.Queries.ListUsers;

public sealed record ListUsersQuery(
    int Page = 1,
    int PageSize = 20,
    UserRole? Role = null,
    bool? IsActive = null,
    string? SearchTerm = null);