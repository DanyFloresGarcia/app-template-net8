using Serilog;
using AspNetCoreRateLimit;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Setting CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowApiTemplate",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        //Setting Serilog
        ConfigureSerilog(configuration);

        //Setting ApiKey
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Name = "x-api-key",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
            });
        });

        //Configurar Rate Limiting
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimitOptions"));
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        
        return services;
    }

    private static void ConfigureSerilog(IConfiguration configuration)
    {
        var logToFile = bool.TryParse(configuration["UseFile"], out var consoleEnabled) && consoleEnabled;

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteToFileOrConsole(configuration["Logs__Path"]!, logToFile)
            .CreateLogger();
    }

    // Método de extensión para escribir en archivo o consola
    private static LoggerConfiguration WriteToFileOrConsole(this LoggerConfiguration loggerConfig, string path, bool toFile)
    {
        return toFile
            ? loggerConfig.WriteTo.File(path)
            : loggerConfig.WriteTo.Console();
    }
}