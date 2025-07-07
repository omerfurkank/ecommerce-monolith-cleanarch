using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;
[NotMapped]
public abstract class DomainEvent : INotification
{
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
}
