using Microsoft.EntityFrameworkCore;

using Domain.Customers;
using Domain.Customers.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
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
        return await _context.Customers.ToListAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await GetByIdAsync(id);
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}