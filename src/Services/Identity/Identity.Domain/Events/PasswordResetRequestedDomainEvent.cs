namespace Identity.Domain.Events;

public sealed record PasswordResetRequestedDomainEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid UserId,
    Email Email) : IDomainEvent;