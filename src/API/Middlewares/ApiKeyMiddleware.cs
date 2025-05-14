namespace API.Middlewares;

public class ApiKeyMiddleware
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyMiddleware> _logger;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            string message = $"API Key was not provided in the request. Header: {ApiKeyHeaderName}";
            _logger.LogWarning(message);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key is missing.");
            return;
        }

        var apiKey = _configuration.GetValue<string>("ApiSettings:ApiKey")!;

        if (!apiKey.Equals(extractedApiKey))
        {
            string message = $"API Key is invalid. Expected: {apiKey}, Received: {extractedApiKey}";
            _logger.LogWarning(message);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key is invalid.");
            return;
        }

        await _next(context);
    }
}
