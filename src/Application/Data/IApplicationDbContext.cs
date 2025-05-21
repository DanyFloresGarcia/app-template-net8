using Microsoft.EntityFrameworkCore;
using Domain.Customers;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Data;
public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; set; }
    ChangeTracker ChangeTracker { get; }
    EntityEntry Entry(object entity);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}