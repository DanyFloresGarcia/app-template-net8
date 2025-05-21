using Microsoft.EntityFrameworkCore;

using Domain.Customers;
using Domain.Customers.Interfaces;
using Domain.ValueObjects;
using Application.Data;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IApplicationDbContext _context;

    public CustomerRepository(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<Customer> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id)
               ?? throw new KeyNotFoundException($"Customer with ID {id} not found.");
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        Console.WriteLine($"DbContext actual: {_context.GetType().Name}");
        return await _context.Customers.ToListAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        if (customer != null)
        {
            await _context.Customers.AddAsync(customer);
        }
        else
        {
            throw new ArgumentNullException(nameof(customer));
        }
    }

    public async Task UpdateAsync(Customer customer, AuditRecord auditRecord)
    {
        Customer customerOld = await GetByIdAsync(customer.Id);

        var trackedEntity = _context.ChangeTracker.Entries<Customer>()
                              .FirstOrDefault(e => e.Entity.Id == customer.Id);

        if (trackedEntity == null)
        {
            var entry = _context.Entry(customer);
            customerOld.Address = customer.Address;
            customerOld.Email = customer.Email;
            customerOld.AuditRecord = auditRecord;
        }
        else
        {
            customer = trackedEntity.Entity;
            customerOld.Address = customer.Address;
            customerOld.Email = customer.Email;
            customer.AuditRecord = auditRecord;
        }
    }

    public async Task DeleteAsync(int id, AuditRecord auditRecord)
    {
        Customer customer = await GetByIdAsync(id);

        var trackedEntity = _context.ChangeTracker.Entries<Customer>()
                                .FirstOrDefault(e => e.Entity.Id == customer.Id);

        if (trackedEntity == null)
        {
            var entry = _context.Entry(customer);
            customer.AuditRecord = auditRecord;
        }
        else
        {
            customer = trackedEntity.Entity;
            customer.AuditRecord = auditRecord;
        }
    }
}