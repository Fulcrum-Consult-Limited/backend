using Identity.API.Extensions;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/password")]
public sealed class PasswordResetController(
    RequestPasswordResetHandler requestHandler,
    ResetPasswordHandler resetHandler,
    ChangePasswordHandler changeHandler) : ControllerBase
{
    // Public — user submits their email
    [HttpPost("forgot")]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken ct)
    {
        await requestHandler.Handle(
            new RequestPasswordResetCommand(request.Email), ct);

        // Always return 200 regardless of outcome — prevents user enumeration
        return Ok(new { message = "If that email exists, a reset link has been sent." });
    }

    // Public — user submits token + new password from the reset link
    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken ct)
    {
        var result = await resetHandler.Handle(
            new ResetPasswordCommand(request.Token, request.NewPassword), ct);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => error.Code switch
            {
                "identity.password_reset.not_found"    => NotFound(error.ToResponse()),
                "identity.password_reset.expired"      => Gone(error.ToResponse()),
                "identity.password_reset.already_used" => Conflict(error.ToResponse()),
                "identity.credentials.weak_password"   => UnprocessableEntity(error.ToResponse()),
                "identity.user.inactive"               => Forbid(),
                _                                      => BadRequest(error.ToResponse())
            });
    }

    // Authenticated — user changes their own password
    [Authorize]
    [HttpPost("change")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();

        var result = await changeHandler.Handle(
            new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword), ct);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => error.Code switch
            {
                "identity.credentials.invalid"       => Unauthorized(error.ToResponse()),
                "identity.credentials.weak_password" => UnprocessableEntity(error.ToResponse()),
                "identity.user.inactive"             => Forbid(),
                _                                    => BadRequest(error.ToResponse())
            });
    }

    private ObjectResult Gone(object value) =>
        StatusCode(StatusCodes.Status410Gone, value);
}

public sealed record ForgotPasswordRequest(string Email);
public sealed record ResetPasswordRequest(string Token, string NewPassword);
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);