using Domain.Primitives;
using Domain.ValueObjects;
namespace Domain.Customers;
public sealed class Customer : AggregateRoot
{
    public int Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public AuditRecord AuditRecord { get; set; }
    private Customer() { }

    public Customer(string name, string lastname, string email, string address, AuditRecord auditRecord)
    {
        Name = name;
        LastName = lastname;
        Email = email;
        Address = address;
        AuditRecord = auditRecord ?? throw new ArgumentNullException(nameof(auditRecord));
    }

    // Method for updating an existing customer
    public void Update(string name, string email)
    {
        Name = name;
        Email = email;
    }
}