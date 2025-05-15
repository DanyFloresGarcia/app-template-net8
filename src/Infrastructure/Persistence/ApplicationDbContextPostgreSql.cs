using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Configuration.PostgreSql;


namespace Infrastructure.Persistence;

public class ApplicationDbContextPostgreSql : ApplicationDbContext
{
    public ApplicationDbContextPostgreSql(DbContextOptions<ApplicationDbContextPostgreSql> options, IPublisher publisher)
        : base(options, publisher) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CustomerConfigurationPostgreSql).Assembly);
    }
}