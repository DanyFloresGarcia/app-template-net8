using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContextPostgreSql : ApplicationDbContext
{
    public ApplicationDbContextPostgreSql(DbContextOptions<ApplicationDbContextPostgreSql> options, IPublisher publisher)
        : base(options, publisher) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(Configuration.PostgreSql.CustomerConfigurationPostgreSql).Assembly);
    }
}