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

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateInvitadoLambda;

public class Function
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IConfiguration _configuration;

	public Function()
	{
		var services = new ServiceCollection();

		services.AddLogging(config =>
		{
			config.AddConsole();
		});


		var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables();

		_configuration = builder.Build();


		services.AddSingleton<IConfiguration>(_configuration);
		services.AddApplication();
		services.AddInfrastructure(_configuration);

		_serviceProvider = services.BuildServiceProvider();
	}

	public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
	{
		context.Logger.LogLine("🟢 Iniciando Lambda");
		LogToFile("Iniciando Lambda");

		try
		{
			LogToFile($"Request Body: {request.Body}");
			var mediator = _serviceProvider.GetRequiredService<IMediator>();
			var command = JsonConvert.DeserializeObject<CreateCustomerCommand>(request.Body);

			LogToFile("Deserialización completada."); 

			LogToFile("Enviar mensaje CQRS.");
			var result = await mediator.Send(command!);

			LogToFile("Respuesta de CQRS.");

			var response = new APIGatewayProxyResponse
			{
				StatusCode = 201,
				Body = result.Value.ToString(),
				Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
			};

			LogToFile($"Resultado: {result.Value}");

			return response;
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
		}
	}

	private void LogToFile(string message)
	{
		var logPath = Path.Combine(Directory.GetCurrentDirectory(), "log-lambda.txt");
		File.AppendAllText(logPath, $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
	}
}
