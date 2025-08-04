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
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Application.Auth.Login;
using Helpers;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LoginSingleClientLambda;

public class Function
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IConfiguration _configuration;
    private readonly ILogger<Function> _logger;

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
        _logger = _serviceProvider.GetRequiredService<ILogger<Function>>();
	}

	public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
		{
			_logger.LogInformation($"Request Body: {request.Body}\n");
			var mediator = _serviceProvider.GetRequiredService<IMediator>();

			_logger.LogInformation("Deserialización completada.\n");
			var command = JsonConvert.DeserializeObject<LoginCommand>(request.Body);

			_logger.LogInformation("Enviando mensaje CQRS.\n");
			var result = await mediator.Send(command!);

			_logger.LogInformation("Recibiendo respuesta CQRS.\n");

			return result.Match(
				userId => LambdaResponse.Success(new { UserId = userId }, 201),
				errors => LambdaResponse.Error(errors)
			);
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
			Log.CloseAndFlush();
		}
    }
 
}
