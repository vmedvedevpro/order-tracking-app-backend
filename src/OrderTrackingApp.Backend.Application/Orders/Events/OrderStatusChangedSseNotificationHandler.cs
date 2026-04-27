using MediatR;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;

namespace OrderTrackingApp.Backend.Application.Orders.Events;

internal sealed record OrderStatusChangedSseNotificationHandler(IOrderStatusNotifier Notifier)
    : INotificationHandler<OrderStatusChangedDomainEvent>
{
    public async Task Handle(OrderStatusChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderStatusChangedIntegrationEvent
                               {
                                   OrderNumber = notification.OrderNumber,
                                   OldStatus = notification.OldStatus.ToString(),
                                   NewStatus = notification.NewStatus.ToString(),
                                   OccurredAt = notification.OccurredAt
                               };

        await Notifier.NotifyAsync(integrationEvent, cancellationToken);
    }
}
