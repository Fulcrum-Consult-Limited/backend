namespace Identity.Application.Setup.Commands.Bootstrap;

public sealed record BootstrapCommand(
    string Email,
    string Forename,
    string Surname,
    string Password);