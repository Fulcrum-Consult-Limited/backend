namespace Identity.API.Controllers;

[ApiController]
[Route("api/setup")]
public sealed class SetupController(BootstrapHandler bootstrapHandler) : ControllerBase
{
    [HttpPost("bootstrap")]
    public async Task<IActionResult> Bootstrap(
        [FromBody] BootstrapRequest request,
        CancellationToken ct)
    {
        var result = await bootstrapHandler.Handle(
            new BootstrapCommand(
                request.Email,
                request.Forename,
                request.Surname,
                request.Password), ct);

        return result.Match<IActionResult>(
            onSuccess: user => CreatedAtAction(nameof(Bootstrap), user),
            onFailure: error => error.Code switch
            {
                "identity.setup.already_completed"   => NotFound(),
                "identity.credentials.invalid_email" => BadRequest(error.ToResponse()),
                "identity.credentials.weak_password" => UnprocessableEntity(error.ToResponse()),
                _                                    => BadRequest(error.ToResponse())
            });
    }
}

public sealed record BootstrapRequest(
    string Email,
    string Forename,
    string Surname,
    string Password);