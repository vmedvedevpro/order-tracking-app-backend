using MassTransit;

using OrderTrackingApp.Backend.Application.Common.Interfaces;

namespace OrderTrackingApp.Backend.Infrastructure.Messaging;

internal sealed class MassTransitEventBus(ISendEndpointProvider sendEndpointProvider) : IEventBus
{
    private static readonly Uri BackendOutputEndpoint = new("queue:BackendOutput");

    public async Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        var endpoint = await sendEndpointProvider.GetSendEndpoint(BackendOutputEndpoint);

        await endpoint.Send(integrationEvent, cancellationToken);
    }
}
