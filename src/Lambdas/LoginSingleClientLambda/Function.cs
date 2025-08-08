using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.DependencyInjection;
using Application;
using Infrastructure;
using MediatR;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Application.Auth.Login;
using Application.Auth.Logout;
using Application.Auth.RefreshToken;
using Helpers;
using System.Runtime.Loader;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LoginSingleClientLambda;

public class Function
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IConfiguration _configuration;
	private readonly ILogger<Function> _logger;
	private static bool _assemblyHooked;

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

		services.AddSingleton<IConfiguration>(_configuration);
		services.AddApplication();
		services.AddInfrastructure(_configuration);

		_serviceProvider = services.BuildServiceProvider();
		_logger = _serviceProvider.GetRequiredService<ILogger<Function>>();
	}

	public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request)
	{
		try
		{
			_logger.LogInformation("Request All: {Request}", JsonConvert.SerializeObject(request));

			_logger.LogInformation($"Request Body: {request.Body}\n");
			_logger.LogInformation($"Request Path: {request.RawPath}\n");

			string stageApi = _configuration["Api:Stage"]!.ToString();
			_logger.LogInformation($"Stage API: {stageApi}\n");

			using (var scope = _serviceProvider.CreateScope())
			{
				if (request.RawPath == $"{stageApi}/login")
				{
					return await HandleLogin(request);
				}
				else if (request.RawPath == $"{stageApi}/logout")
				{
					return await HandleLogout(request);
				}
				else if (request.RawPath == $"{stageApi}/refresh-token")
				{
					return await HandleRefreshToken(request);
				}
				else
				{
					return new APIGatewayHttpApiV2ProxyResponse
					{
						StatusCode = 404,
						Body = $"Ruta no encontrada"
					};
				}
			}
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error: {ex.Message}");
			_logger.LogError($"StackTrace: {ex.StackTrace}");

			return new APIGatewayHttpApiV2ProxyResponse
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

	private async Task<APIGatewayHttpApiV2ProxyResponse> HandleLogin(APIGatewayHttpApiV2ProxyRequest request)
	{
		var mediator = _serviceProvider.GetRequiredService<IMediator>();
		var command = JsonConvert.DeserializeObject<LoginCommand>(request.Body);
		var result = await mediator.Send(command!);

		return result.Match(
			userId => LambdaResponse.Success(new { UserId = userId }, 201),
			errors => LambdaResponse.Error(errors)
		);
	}

	private async Task<APIGatewayHttpApiV2ProxyResponse> HandleLogout(APIGatewayHttpApiV2ProxyRequest request)
	{
		var mediator = _serviceProvider.GetRequiredService<IMediator>();
		var command = JsonConvert.DeserializeObject<LogoutCommand>(request.Body);
		var result = await mediator.Send(command!);

		return result.Match(
			success => LambdaResponse.Success(new { Message = "Sesión cerrada" }, 200),
			errors => LambdaResponse.Error(errors)
		);
	}

	private async Task<APIGatewayHttpApiV2ProxyResponse> HandleRefreshToken(APIGatewayHttpApiV2ProxyRequest request)
	{
		var mediator = _serviceProvider.GetRequiredService<IMediator>();
		var command = JsonConvert.DeserializeObject<RefreshTokenCommand>(request.Body);
		var result = await mediator.Send(command!);

		return result.Match(
			tokens => LambdaResponse.Success(tokens, 200),
			errors => LambdaResponse.Error(errors)
		);
	}

}
