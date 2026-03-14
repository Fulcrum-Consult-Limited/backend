namespace Identity.Domain.Events;

public sealed record InvitationCreatedDomainEvent(
    Guid EventId,
    DateTime OccurredAt,
    Guid UserId,
    Email Email) : IDomainEvent;