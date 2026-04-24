using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OrderTrackingApp.Backend.Infrastructure.Persistence;

public static class InitializerExtensions
{
    public static async Task InitialiseDatabaseAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

        await initializer.InitialiseAsync();
    }
}

public record DatabaseInitializer(DatabaseContext DatabaseContext, ILogger<DatabaseInitializer> Logger)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await DatabaseContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }
    }
}
