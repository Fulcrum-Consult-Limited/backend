namespace Identity.Application.Auth.Commands.Logout;

public sealed class LogoutHandler(
    IJwtService jwtService,
    ITokenRevocationStore revocationStore)
{
    public async Task<Result> Handle(
        LogoutCommand command,
        CancellationToken ct = default)
    {
        var (jti, expiresAt) = jwtService.ExtractClaims(command.AccessToken);

        var remainingTtl = expiresAt - DateTime.UtcNow;
        if (remainingTtl <= TimeSpan.Zero)
            return Result.Success();

        await revocationStore.RevokeAsync(jti, remainingTtl, ct);

        return Result.Success();
    }
}