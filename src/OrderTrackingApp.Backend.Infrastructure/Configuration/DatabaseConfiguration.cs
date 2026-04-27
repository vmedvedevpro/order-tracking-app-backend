using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Infrastructure.Persistence;

namespace OrderTrackingApp.Backend.Infrastructure.Configuration;

internal static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var dataSource = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Database"))
                         .EnableDynamicJson()
                         .ConfigureJsonOptions(new JsonSerializerOptions { AllowOutOfOrderMetadataProperties = true })
                         .EnableParameterLogging().Build();

        return services.AddDbContext<DatabaseContext>(o => o.EnableSensitiveDataLogging().EnableDetailedErrors().UseNpgsql(dataSource))
                       .AddScoped<IDatabaseContext>(provider => provider.GetRequiredService<DatabaseContext>())
                       .AddTransient<DatabaseInitializer>();
    }
}
