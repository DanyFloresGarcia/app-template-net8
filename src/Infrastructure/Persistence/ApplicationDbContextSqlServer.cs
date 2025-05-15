using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Configuration.SqlServer;

namespace Infrastructure.Persistence;

public class ApplicationDbContextSqlServer : ApplicationDbContext
{
    public ApplicationDbContextSqlServer(DbContextOptions<ApplicationDbContextSqlServer> options, IPublisher publisher)
        : base(options, publisher) { }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CustomerConfigurationSqlServer).Assembly);
    }
}