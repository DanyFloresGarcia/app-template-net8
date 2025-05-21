using Microsoft.EntityFrameworkCore;
using Application.Data;

namespace API.Extensions;

public static class MigrationExtensions{
    public static void ApplyMigrations(this WebApplication app){

        Console.WriteLine("Applying migrations...");
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();        
        dbContext.Database.Migrate();
    }
}