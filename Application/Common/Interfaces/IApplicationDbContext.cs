using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    //DbSet<Category> Categories { get; }
    //DbSet<Brand> Brands { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}
