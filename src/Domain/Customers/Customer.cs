using Domain.Primitives;
using Domain.ValueObjects;
namespace Domain.Customers;
public sealed class Customer : AggregateRoot
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public AuditRecord AuditRecord { get; private set; }

    private Customer() { }

    // Constructor for creating a new customer
    public Customer(string name, string email, AuditRecord auditRecord)
    {
        Name = name;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        AuditRecord = auditRecord ?? throw new ArgumentNullException(nameof(auditRecord));
    }

    // Method for updating an existing customer
    public void Update(string name, string email)
    {
        Name = name;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }
}