using MediatR;
using Microsoft.EntityFrameworkCore;

using Domain.Customers;
using Application.Data;
using Infrastructure.Persistence.Configuration.MongoDb;

namespace Infrastructure.Persistence;

public class ApplicationDbContextMongoDb : ApplicationDbContext, IApplicationDbContext
{
    public ApplicationDbContextMongoDb(DbContextOptions<ApplicationDbContextMongoDb> options, IPublisher publisher)
        : base(options, publisher) { }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Console.WriteLine("Instanciando ApplicationDbContextMongoDb");
        modelBuilder.ApplyConfiguration(new CustomerConfigurationMongoDb()); 
    }
}