namespace Identity.Domain.Events;

public sealed record UserRegisteredDomainEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid UserId,
    Email Email) : IDomainEvent;