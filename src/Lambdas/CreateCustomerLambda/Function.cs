using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.DependencyInjection;
using Application;
using Infrastructure;
using MediatR;
using Newtonsoft.Json;
using Application.Customers.Create;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Helpers;
using System.Runtime.Loader;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateCustomerLambda;

public class Function
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IConfiguration _configuration;
	private static bool _assemblyHooked;
	private readonly ILogger<Function> _logger;

	public Function()
	{
		if (!_assemblyHooked)
		{
			Console.WriteLine("Iniciando registro desde Resolving.");
			AssemblyLoadContext.Default.Resolving += (context, assemblyName) =>
			{
				var path = Path.Combine("/opt", $"{assemblyName.Name}.dll");
				Console.WriteLine($"[Resolving] Buscando DLL: {path}");
				if (File.Exists(path))
				{
					Console.WriteLine($"[Resolving] Cargando: {path}");
					return context.LoadFromAssemblyPath(path);
				}
				Console.WriteLine($"[Resolving] No se encontró: {path}");
				return null;
			};

			Console.WriteLine("AssemblyLoadContext registrado desde staticctor.");
			_assemblyHooked = true;
		}

		var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables();

		_configuration = builder.Build();
		var services = new ServiceCollection();

		services.AddLogging(logging =>
		{
			logging.ClearProviders();
			logging.AddConsole();
			logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
		});

		services.AddSingleton<IConfiguration>(_configuration)
			.AddApplication()
			.AddPersistence(_configuration);

		_serviceProvider = services.BuildServiceProvider();
		_logger = _serviceProvider.GetRequiredService<ILogger<Function>>();
	}

	public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
	{
		_logger.LogInformation("Iniciando Lambda");
		_logger.LogInformation("Request All: {Request}", JsonConvert.SerializeObject(request));

		try
		{
			_logger.LogInformation($"Request Body: {request.Body}");
			using (var scope = _serviceProvider.CreateScope())
			{
				var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
				_logger.LogInformation("Deserialización completada.");
				var command = JsonConvert.DeserializeObject<CreateCustomerCommand>(request.Body);

				_logger.LogInformation("Enviar mensaje CQRS.");
				var result = await mediator.Send(command!);
				_logger.LogInformation("Mensaje CQRS enviado.");

				return result.Match(
					customerId => LambdaResponse.Success(new { CustomerId = customerId }, 201),
					errors => LambdaResponse.Error(errors)
				);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error: {ex.Message}");
			_logger.LogError($"StackTrace: {ex.StackTrace}");

			return new APIGatewayProxyResponse
			{
				StatusCode = 500,
				Body = $"Error interno: {ex.Message}"
			};
		}
		finally
		{
			_logger.LogInformation("Finalizando Lambda");
		}
	}
}
