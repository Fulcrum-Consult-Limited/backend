namespace Identity.Domain.Exceptions;

public sealed class WeakPasswordException : DomainException
{
    public IReadOnlyList<string> Violations { get; }

    public WeakPasswordException(IReadOnlyList<string> violations)
        : base($"Password does not meet complexity requirements: {string.Join(", ", violations)}.")
    {
        Violations = violations;
    }
}