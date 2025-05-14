using Microsoft.EntityFrameworkCore;
using Domain.Customers;

namespace Application.Data;
public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; set; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}