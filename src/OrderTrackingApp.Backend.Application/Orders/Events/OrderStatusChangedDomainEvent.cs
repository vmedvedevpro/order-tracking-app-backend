using MediatR;

using OrderTrackingApp.Backend.Domain.Enums;

namespace OrderTrackingApp.Backend.Application.Orders.Events;

public record OrderStatusChangedDomainEvent(
    long OrderNumber,
    OrderStatus OldStatus,
    OrderStatus NewStatus,
    DateTimeOffset OccurredAt) : INotification;
