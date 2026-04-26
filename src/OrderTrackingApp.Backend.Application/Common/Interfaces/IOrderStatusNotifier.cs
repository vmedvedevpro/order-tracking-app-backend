using OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;

namespace OrderTrackingApp.Backend.Application.Common.Interfaces;

public interface IOrderStatusNotifier
{
    ValueTask NotifyAsync(OrderStatusChangedIntegrationEvent statusChanged, CancellationToken cancellationToken);
}
