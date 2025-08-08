using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Application.Data;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence;

public abstract class ApplicationDbContext : DbContext, IUnitOfWork
{
    public readonly IPublisher _publiser;
    private IDbContextTransaction? _currentTransaction;

    public ApplicationDbContext(DbContextOptions options, IPublisher publiser) : base(options)
    {
        _publiser = publiser ?? throw new ArgumentNullException(nameof(publiser));
    } 

    public void ClearChangeTracker()
    {
        ChangeTracker.Clear();  
    }

    public IEnumerable<EntityEntry> GetTrackedEntries()
    {
        return ChangeTracker.Entries();
    }
    protected abstract override void OnModelCreating(ModelBuilder modelBuilder);
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var domainEvents = ChangeTracker.Entries<AggregateRoot>()
        .Select(e => e.Entity)
        .Where(e => e.GetDomainEvents().Any())
        .SelectMany(e => e.GetDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _publiser.Publish(domainEvent, cancellationToken);
        }

        return result;
    }



    // Metodos para manejar las transacciones
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            return;
        }

        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

}