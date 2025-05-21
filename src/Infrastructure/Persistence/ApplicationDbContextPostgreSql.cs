using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Configuration.PostgreSql;

using Domain.Customers;
using Application.Data;

namespace Infrastructure.Persistence;

public class ApplicationDbContextPostgreSql : ApplicationDbContext, IApplicationDbContext
{
    public ApplicationDbContextPostgreSql(DbContextOptions<ApplicationDbContextPostgreSql> options, IPublisher publisher)
        : base(options, publisher) { }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Console.WriteLine("Instanciando ApplicationDbContextPostgreSql");
        modelBuilder.ApplyConfiguration(new CustomerConfigurationPostgreSql()); 
    }
}