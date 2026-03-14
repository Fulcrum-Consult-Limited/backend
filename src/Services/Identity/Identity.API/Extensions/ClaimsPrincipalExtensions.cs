namespace Identity.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue("sub")
            ?? throw new InvalidOperationException("User ID claim not found.");

        return Guid.Parse(value);
    }

    public static string GetEmail(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Email)
            ?? principal.FindFirstValue("email")
            ?? throw new InvalidOperationException("Email claim not found.");

    public static string GetJti(this ClaimsPrincipal principal) =>
        principal.FindFirstValue("jti")
            ?? throw new InvalidOperationException("JTI claim not found.");
}