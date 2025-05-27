using Domain.ValueObjects;

namespace Domain.Customers.Interfaces;
public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer, AuditRecord auditRecord);
    Task DeleteAsync(int id, AuditRecord auditRecord);
}