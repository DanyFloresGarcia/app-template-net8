using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Configuration.Mysql;
using Domain.Customers;
using Application.Data;

namespace Infrastructure.Persistence;

public class ApplicationDbContextMySql : ApplicationDbContext, IApplicationDbContext
{
    public ApplicationDbContextMySql(DbContextOptions<ApplicationDbContextMySql> options, IPublisher publisher)
        : base(options, publisher) { }    
    
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Console.WriteLine("Instanciando ApplicationDbContextMySql");
        
        modelBuilder.ApplyConfiguration(new CustomerConfigurationMysql()); 
    }
}