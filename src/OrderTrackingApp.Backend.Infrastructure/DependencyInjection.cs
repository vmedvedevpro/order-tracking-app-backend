using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OrderTrackingApp.Backend.Infrastructure.Configuration;
using OrderTrackingApp.Backend.Infrastructure.Messaging;

namespace OrderTrackingApp.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostBuilder hostBuilder) =>
        services.AddDatabase(configuration)
                .AddMessaging(configuration)
                .AddTelemetry("OrderTrackingApp.Backend");
}
