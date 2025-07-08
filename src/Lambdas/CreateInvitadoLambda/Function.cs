using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.DependencyInjection;
using Application; // Asegúrate que este namespace es correcto
using Infrastructure; // Asegúrate que este namespace es correcto
using MediatR;
using System.Text.Json;
using Application.Customers.Create;

// Requerido por Lambda
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CreateInvitadoLambda;

public class Function
{
    private readonly IServiceProvider _serviceProvider;

    public Function()
    {
        var services = new ServiceCollection();
        
        // Registrar Application, Infrastructure, etc.
        services.AddApplication();
        //services.AddInfrastructure(); // si es necesario

        _serviceProvider = services.BuildServiceProvider();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            // Suponiendo que estás enviando un CreateInvitadoCommand como JSON en el body
            var command = JsonSerializer.Deserialize<CreateCustomerCommand>(request.Body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var result = await mediator.Send(command!);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(result),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error: {ex.Message}");

            return new APIGatewayProxyResponse
            {
                StatusCode = 500,
                Body = $"Error interno: {ex.Message}"
            };
        }
    }
}
