using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Infrastructure.Messaging.Configuration;
using OrderTrackingApp.Backend.Infrastructure.Persistence;

namespace OrderTrackingApp.Backend.Infrastructure.Messaging;

internal static class MessagingConfiguration
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));

        services.AddMassTransit(cfg =>
                                {
                                    cfg.AddEntityFrameworkOutbox<DatabaseContext>(o =>
                                                                                  {
                                                                                      o.UsePostgres();
                                                                                      o.UseBusOutbox();
                                                                                  });

                                    cfg.UsingRabbitMq((context, busCfg) =>
                                                      {
                                                          var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                                                          busCfg.Host(
                                                              options.Host,
                                                              options.Port,
                                                              options.VirtualHost,
                                                              h =>
                                                              {
                                                                  h.Username(options.Username);
                                                                  h.Password(options.Password);
                                                              });

                                                          busCfg.UseRawJsonSerializer(RawSerializerOptions.AddTransportHeaders);
                                                      });
                                });

        return services.AddScoped<IEventBus, MassTransitEventBus>();
    }
}
