using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContextSqlServer : ApplicationDbContext
{
    public ApplicationDbContextSqlServer(DbContextOptions<ApplicationDbContextSqlServer> options, IPublisher publisher)
        : base(options, publisher) { }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(Configuration.SqlServer.CustomerConfiguration).Assembly);
    }
}