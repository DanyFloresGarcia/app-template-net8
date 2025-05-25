using Domain.Customers;
using Domain.Customers.Interfaces;
using Domain.ValueObjects;
using Application.Data;
using Dapper;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepositoryDapper : ICustomerRepository
{
    private readonly IDapperDbContext _context;

    public CustomerRepositoryDapper(IDapperDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM dflores.Customer WHERE ID = :Id";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id })
               ?? throw new KeyNotFoundException($"Customer with ID {id} not found.");
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        const string sql = "SELECT * FROM dflores.Customer";

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Customer>(sql);
    }

    public async Task AddAsync(Customer customer)
    {
        const string sql = @"INSERT INTO dflores.Customer (ID, NAME, EMAIL, ADDRESS) 
                             VALUES (:Id, :Name, :Email, :Address)";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Address
        });
    }

    public async Task UpdateAsync(Customer customer, AuditRecord auditRecord)
    {
        const string sql = @"UPDATE dflores.Customer 
                             SET NAME = :Name, EMAIL = :Email, ADDRESS = :Address
                             WHERE ID = :Id";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            customer.Name,
            customer.Email,
            customer.Address,
            customer.Id
        });
    }

    public async Task DeleteAsync(int id, AuditRecord auditRecord)
    {
        const string sql = @"DELETE FROM dflores.Customer WHERE ID = :Id";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}