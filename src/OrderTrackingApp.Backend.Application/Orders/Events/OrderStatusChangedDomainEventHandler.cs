using MediatR;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;

namespace OrderTrackingApp.Backend.Application.Orders.Events;

internal sealed record OrderStatusChangedDomainEventHandler(IEventBus EventBus)
    : INotificationHandler<OrderStatusChangedDomainEvent>
{
    public Task Handle(OrderStatusChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderStatusChangedIntegrationEvent
                               {
                                   OrderNumber = notification.OrderNumber,
                                   OldStatus = notification.OldStatus.ToString(),
                                   NewStatus = notification.NewStatus.ToString(),
                                   OccurredAt = notification.OccurredAt
                               };

        return EventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
