namespace Identity.Application.PasswordReset.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    string Token,
    string NewPassword);