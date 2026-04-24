using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace OrderTrackingApp.Backend.Application;

public static class DependencyInjection
{
    private static readonly Assembly Assembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly));
}
