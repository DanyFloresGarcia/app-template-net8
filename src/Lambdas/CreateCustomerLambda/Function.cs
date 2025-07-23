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
using Serilog;
using Helpers;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateCustomerLambda;

public class Function
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IConfiguration _configuration;

	public Function()
	{
		Log.Logger = new LoggerConfiguration()
		.MinimumLevel.Information()
		.Enrich.FromLogContext()
		.WriteTo.Console()
		.CreateLogger();

		var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables();

		_configuration = builder.Build();
		var services = new ServiceCollection();
 
		services.AddLogging(logging =>
		{
			logging.ClearProviders();
			logging.AddSerilog(dispose: true);
		});

		services.AddSingleton<IConfiguration>(_configuration);
		services.AddApplication();
		services.AddInfrastructure(_configuration);

		_serviceProvider = services.BuildServiceProvider();
	}

	public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
	{
		LogToFile("Iniciando Lambda");

		try
		{
			LogToFile($"All Request: {request}");
			LogToFile($"Request Body: {request.Body}");
			var mediator = _serviceProvider.GetRequiredService<IMediator>();

			LogToFile("Deserialización completada.");
			var command = JsonConvert.DeserializeObject<CreateCustomerCommand>(request.Body);

			LogToFile("Enviar mensaje CQRS.");
			var result = await mediator.Send(command!);
	
			LogToFile("Mensaje CQRS enviado.");

			return result.Match(
				customerId => LambdaResponse.Success(new { CustomerId = customerId }, 201),
				errors => LambdaResponse.Error(errors)
			);
		}
		catch (Exception ex)
		{
			LogToFile($"Error: {ex.Message}");
			LogToFile($"StackTrace: {ex.StackTrace}");

			return new APIGatewayProxyResponse
			{
				StatusCode = 500,
				Body = $"Error interno: {ex.Message}"
			};
		}finally
		{
			Log.CloseAndFlush();
		}
	}

	private void LogToFile(string message)
	{
		var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

		Console.WriteLine($"Environment: {environment}");

		if (environment == "Development")
		{
			var logPath = Path.Combine(Directory.GetCurrentDirectory(), "log-lambda.txt");
			File.AppendAllText(logPath, $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
		}
		else
		{
			Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
		}
	} 
}
