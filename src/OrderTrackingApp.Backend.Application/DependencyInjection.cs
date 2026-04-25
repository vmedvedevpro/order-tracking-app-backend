using System.Reflection;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using OrderTrackingApp.Backend.Application.Common.Behaviors;

namespace OrderTrackingApp.Backend.Application;

public static class DependencyInjection
{
    private static readonly Assembly Assembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services.AddValidatorsFromAssembly(Assembly)
                .AddMediatR(c =>
                            {
                                c.RegisterServicesFromAssembly(Assembly);
                                c.AddOpenBehavior(typeof(ValidationBehavior<,>));
                            });
}
