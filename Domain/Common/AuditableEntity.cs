namespace Domain.Common;

public abstract class AuditableEntity<TId> : Entity<TId>
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public string? CreatedBy { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }
}