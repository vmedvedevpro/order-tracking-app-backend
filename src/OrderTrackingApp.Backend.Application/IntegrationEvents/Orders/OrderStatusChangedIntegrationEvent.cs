namespace OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;

public record OrderStatusChangedIntegrationEvent
{
    public long OrderNumber { get; init; }

    public string OldStatus { get; init; } = null!;

    public string NewStatus { get; init; } = null!;

    public DateTimeOffset OccurredAt { get; init; }
}
