using Domain.Common.Interfaces;
using Domain.Product.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace Infrastructure.Data.Interceptors;

public sealed class AuditableEntityInterceptor(IUserContext userContext) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null) return;

        var now = DateTime.UtcNow;
        var userId = userContext.UserId;

        var entries = context.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userId;
            }

            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = userId;
            }

            HandleSoftDeleteIfNeeded(entry, now, userId);
        }
    }

    private void HandleSoftDeleteIfNeeded(EntityEntry<IAuditableEntity> entry, DateTime now, Guid userId)
    {
        var type = entry.Entity.GetType();
        var statusProp = type.GetProperty("Status");

        if (statusProp is null || !statusProp.PropertyType.IsEnum)
            return;

        // Check if enum has [SoftDeletableStatus] attribute
        var isSoftDeletable = statusProp.PropertyType.GetCustomAttribute<SoftDeletableStatusAttribute>() is not null;
        if (!isSoftDeletable)
            return;

        var statusValue = statusProp.GetValue(entry.Entity);
        if (statusValue is null)
            return;

        var statusName = Enum.GetName(statusProp.PropertyType, statusValue);
        if (statusName == "Deleted")
        {
            entry.Entity.DeletedAt ??= now;
            entry.Entity.DeletedBy ??= userId;
        }
    }
}

public static class EntityEntryExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry is { State: EntityState.Added or EntityState.Modified } &&
            r.TargetEntry.Metadata.IsOwned());
}
