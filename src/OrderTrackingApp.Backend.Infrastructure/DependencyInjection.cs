using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Npgsql;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Infrastructure.Messaging;
using OrderTrackingApp.Backend.Infrastructure.Persistence;

namespace OrderTrackingApp.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostBuilder hostBuilder) =>
        services.AddDatabase(configuration)
                .AddMessaging(configuration);

    private static IServiceCollection AddDatabase(
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
