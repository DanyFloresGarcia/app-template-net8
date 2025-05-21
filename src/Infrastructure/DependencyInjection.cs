using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Application.Common.Mappings;
using Domain.Customers.Interfaces;
using Domain.Primitives;
using Application.Data;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Common;

namespace Infrastructure;

public static class DependencyInjection
{
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
                services.AddPersistence(configuration);
                return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
                var databaseSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>()!;
                Console.WriteLine(databaseSettings.Provider);
                Console.WriteLine(databaseSettings.ConnectionString);
                switch (databaseSettings.Provider)
                {
                        case DatabaseProvider.SqlServer:
                                AddDbContextSqlServer(services, databaseSettings.ConnectionString);
                                break;
                        case DatabaseProvider.PostgreSql:
                                AddDbContextPostgreSql(services, databaseSettings.ConnectionString);
                                break;
                }

                //AutoMapper
                services.AddAutoMapper(typeof(MappingProfile));

                // services.AddScoped<IUnitOfWork>(sp =>
                //         sp.GetRequiredService<IApplicationDbContext>());

                //Repository
                services.AddScoped<ICustomerRepository, CustomerRepository>();

                //Domain

                //Services

                //Singleton
                //services.AddSingleton<IVaultCredentialsProvider, VaultCredentialsProvider>();

                //services.AddTransient<SomeApplication>();

                return services;
        }

        private static void AddDbContextSqlServer(IServiceCollection services, string connectionString)
        {
                services.AddDbContext<ApplicationDbContextSqlServer>(options =>
                                        options.UseSqlServer(connectionString));
                                services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContextSqlServer>());
                                services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContextSqlServer>());
        }
        private static void AddDbContextPostgreSql(IServiceCollection services, string connectionString)
        {
                services.AddDbContext<ApplicationDbContextPostgreSql>(options =>
                                        options.UseNpgsql(connectionString));
                                services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContextPostgreSql>());
                                services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContextPostgreSql>());
        }
}