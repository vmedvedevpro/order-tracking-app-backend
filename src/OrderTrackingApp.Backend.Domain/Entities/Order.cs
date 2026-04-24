using OrderTrackingApp.Backend.Domain.Enums;

namespace OrderTrackingApp.Backend.Domain.Entities;

public class Order
{
    public ulong OrderNumber { get; set; }

    public string Description { get; set; } = null!;

    public OrderStatus Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
