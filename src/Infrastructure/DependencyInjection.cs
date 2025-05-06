
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Application.Common.Mappings;
using Domain.Customers.Interfaces;
using Domain.Primitives;
using Application.Data;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure;

public static class DependencyInjection{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration){
        services.AddPersistence(configuration);
        return services;
    }

     public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration){
        //ConnectionString
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        //EntityFramework
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        //Dapper
        //services.AddSingleton<DapperContext>(sp =>
        //{
        //    return new DapperContext(connectionString);
        //});
        
        //AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));
        
        services.AddScoped<IApplicationDbContext>(sp => 
                sp.GetRequiredService<ApplicationDbContext>()
        );

        services.AddScoped<IUnitOfWork>(sp => 
                sp.GetRequiredService<ApplicationDbContext>());

        //Repository
        services.AddScoped<ICustomerRepository, CustomerRepository>();

		//Domain

		//Services

		//Singleton
		//services.AddSingleton<IVaultCredentialsProvider, VaultCredentialsProvider>();

        //services.AddTransient<SomeApplication>();

        return services;
    }
}