using Amazon.Lambda.APIGatewayEvents;
using ErrorOr;
using Newtonsoft.Json;

namespace Helpers;

public static class LambdaResponse
{
    public static APIGatewayProxyResponse Success(object body, int statusCode = 200) =>
        new()
        {
            StatusCode = statusCode,
            Body = JsonConvert.SerializeObject(body),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };

    public static APIGatewayProxyResponse Error(List<Error> errors, int statusCode = 400) =>
        new()
        {
            StatusCode = statusCode,
            Body = JsonConvert.SerializeObject(new
            {
                Success = false,
                Errors = errors.Select(e => new { e.Code, e.Description })
            }),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
}
