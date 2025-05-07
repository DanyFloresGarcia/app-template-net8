
using AspNetCoreRateLimit;
using Serilog;

using API;
using API.Middlewares;
using Infrastructure;
using Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration)
                .AddInfrastructure(builder.Configuration)
                .AddApplication();

//Use Serilog for logging
builder.Host.UseSerilog();


var app = builder.Build();

app.UseCors("AllowApiTemplate");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("User-Agent", httpContext.Request.Headers["User-Agent"].ToString());
    };
});

//app.UseExceptionHandler("/error");
Console.WriteLine("Connection string: " + builder.Configuration.GetConnectionString("DefaultConnection"));
Console.WriteLine("Environment: " + builder.Environment.EnvironmentName);

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

app.UseIpRateLimiting();

app.MapControllers();

app.Run();