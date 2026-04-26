using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OrderTrackingApp.Backend.Infrastructure.Configuration;

public static class TelemetryConfiguration
{
    private const string MassTransitInstrumentationName = "MassTransit";

    public static IServiceCollection AddTelemetry(this IServiceCollection services, string applicationName) =>
        services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                                       .AddService(serviceName: applicationName))
                .WithMetrics(metrics => metrics
                                        .AddAspNetCoreInstrumentation()
                                        .AddProcessInstrumentation()
                                        .AddRuntimeInstrumentation()
                                        .AddHttpClientInstrumentation()
                                        .AddMeter(MassTransitInstrumentationName)
                                        .AddPrometheusExporter())
                .WithTracing(tracing => tracing
                                        .AddAspNetCoreInstrumentation()
                                        .AddHttpClientInstrumentation()
                                        .AddNpgsql()
                                        .AddSource(MassTransitInstrumentationName)
                                        .AddOtlpExporter())
                .Services;

    public static IApplicationBuilder UseTelemetry(this IApplicationBuilder app) =>
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
}
