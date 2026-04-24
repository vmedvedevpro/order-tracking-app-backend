using MediatR;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Create;

internal record CreateOrderHandler(IDatabaseContext DatabaseContext) : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = request.Order;
        order.CreatedAt = DateTimeOffset.UtcNow;
        order.UpdatedAt = DateTimeOffset.UtcNow;

        await DatabaseContext.Orders.AddAsync(order, cancellationToken);
        await DatabaseContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
