using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;
using OrderTrackingApp.Backend.Application.Orders.Events;
using OrderTrackingApp.Backend.Domain.Enums;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Events;

public class OrderStatusChangedSseNotificationHandlerTests
{
    [Fact]
    public async Task Handle_ShouldMapDomainEventAndCallNotifier()
    {
        var notifier = Substitute.For<IOrderStatusNotifier>();
        var sut = new OrderStatusChangedSseNotificationHandler(notifier);
        var occurredAt = DateTimeOffset.UtcNow;
        var domainEvent = new OrderStatusChangedDomainEvent(7, OrderStatus.Created, OrderStatus.Shipped, occurredAt);

        await sut.Handle(domainEvent, CancellationToken.None);

        await notifier.Received(1).NotifyAsync(
            Arg.Is<OrderStatusChangedIntegrationEvent>(e =>
                                                           e.OrderNumber == 7
                                                           && e.OldStatus == nameof(OrderStatus.Created)
                                                           && e.NewStatus == nameof(OrderStatus.Shipped)
                                                           && e.OccurredAt == occurredAt),
            Arg.Any<CancellationToken>());
    }
}
