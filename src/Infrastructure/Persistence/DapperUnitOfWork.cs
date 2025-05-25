using System.Threading;
using System.Threading.Tasks;
using Domain.Primitives;

namespace Infrastructure.Common;

public class DapperUnitOfWork : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dapper no tiene seguimiento de cambios, así que retornamos 0 o algún valor fijo si deseas
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        // Si deseas manejar transacciones explícitas, deberías implementar algo real aquí
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
