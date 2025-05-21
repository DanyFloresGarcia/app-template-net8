using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Configuration.SqlServer;
using Domain.Customers;
using Application.Data;

namespace Infrastructure.Persistence;

public class ApplicationDbContextSqlServer : ApplicationDbContext, IApplicationDbContext
{
    public ApplicationDbContextSqlServer(DbContextOptions<ApplicationDbContextSqlServer> options, IPublisher publisher)
        : base(options, publisher) { }    
    
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Console.WriteLine("Instanciando ApplicationDbContextSqlServer");
        
        modelBuilder.ApplyConfiguration(new CustomerConfigurationSqlServer()); 
    }
}