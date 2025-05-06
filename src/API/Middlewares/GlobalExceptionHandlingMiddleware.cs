using System.Net;
using System.Text.Json;

namespace API.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        { 
            /*// Obtener el User-Agent de las cabeceras de la solicitud
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            // Registrar el log con Serilog
            _logger.LogInformation("Received request: {Method} {Url} | User-Agent: {UserAgent}",
                context.Request.Method, context.Request.Path, userAgent);
            */
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                message = "Ocurrió un error inesperado.",
                detail = ex.Message // Solo para debug; quítalo en producción
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
