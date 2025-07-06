using MediatR;

namespace Domain.Common;
public abstract class DomainEvent : INotification
{
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
}
