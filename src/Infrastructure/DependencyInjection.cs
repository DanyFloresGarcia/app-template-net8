using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Domain.Customers.Interfaces;
using Domain.Primitives;
using Application.Common.Mappings;
using Application.Data;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Common;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Infrastructure.Providers;
using Aplication.Data;

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
                Console.WriteLine("Tipo Base datos:" + databaseSettings.Provider);
                Console.WriteLine("Cadena de conexión:" + databaseSettings.ConnectionString);
                switch (databaseSettings.Provider)
                {
                        case DatabaseProvider.SqlServer:
                                AddDbContextSqlServer(services, databaseSettings);
                                break;
                        case DatabaseProvider.PostgreSql:
                                AddDbContextPostgreSql(services, databaseSettings);
                                break;
                        case DatabaseProvider.MongoDb:
                                AddDbContextMongoDb(services, databaseSettings);
                                break;
                        case DatabaseProvider.Mysql:
                                AddDbContextMysqlDb(services, databaseSettings);
                                break;
                        case DatabaseProvider.Oracle:
                                AddDbContextOracleDb(services, databaseSettings);
                                break;
                }

                //AutoMapper
                services.AddAutoMapper(typeof(MappingProfile));

                // services.AddScoped<IUnitOfWork>(sp =>
                // sp.GetRequiredService<IApplicationDbContext>());


                //Domain
                //Services
                services.AddScoped<ILoginService, CognitoAuthProvider>(); 

                //Singleton
                services.AddSingleton<ICredentialsProvider, CredentialsProvider>(); 

                //services.AddTransient<SomeApplication>();

                return services;
        }

        private static void AddDbContextSqlServer(IServiceCollection services, DatabaseSettings databaseSettings)
        {
                services.AddDbContext<ApplicationDbContextSqlServer>(options =>
                                        options.UseSqlServer(databaseSettings.ConnectionString));
                services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContextSqlServer>());
                services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContextSqlServer>());
                
                //Repository
                services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
        private static void AddDbContextPostgreSql(IServiceCollection services, DatabaseSettings databaseSettings)
        {
                services.AddDbContext<ApplicationDbContextPostgreSql>(options =>
                                        options.UseNpgsql(databaseSettings.ConnectionString));
                services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContextPostgreSql>());
                services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContextPostgreSql>());
                //Repository
                services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        private static void AddDbContextMongoDb(IServiceCollection services, DatabaseSettings databaseSettings)
        {
                services.AddDbContext<ApplicationDbContextMongoDb>(options =>
                                        options.UseMongoDB(databaseSettings.ConnectionString, databaseSettings.Database));
                services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContextMongoDb>());
                services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContextMongoDb>());
                //Repository
                services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        private static void AddDbContextMysqlDb(IServiceCollection services, DatabaseSettings databaseSettings)
        {
                services.AddDbContext<ApplicationDbContextMySql>(options =>
                                        options.UseMySql(
                                                databaseSettings.ConnectionString,
                                                ServerVersion.AutoDetect(databaseSettings.ConnectionString)));
                services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContextMySql>());
                services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContextMySql>());
                //Repository
                services.AddScoped<ICustomerRepository, CustomerRepository>();
        }

        private static void AddDbContextOracleDb(IServiceCollection services, DatabaseSettings databaseSettings)
        {
                services.AddScoped<IDapperDbContext>(sp =>
                        new OracleDapperDbContext(databaseSettings.ConnectionString));

                //Repository
                services.AddScoped<ICustomerRepository, CustomerRepositoryDapper>();
                //UnitOfWork
                services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
        }
}