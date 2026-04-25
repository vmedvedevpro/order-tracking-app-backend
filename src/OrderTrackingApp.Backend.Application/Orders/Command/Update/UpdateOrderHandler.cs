using MediatR;

using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Application.Common.Exceptions;
using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Application.Orders.Events;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Update;

internal record UpdateOrderHandler(IDatabaseContext DatabaseContext, IPublisher Publisher)
    : IRequestHandler<UpdateOrderCommand, Order>
{
    public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await DatabaseContext.Orders.FirstOrDefaultAsync(x => x.OrderNumber == request.Order.OrderNumber, cancellationToken);

        if (order == null)
            throw new NotFoundException($"Order '{request.Order.OrderNumber}' not found");

        var previousStatus = order.Status;

        order.Description = request.Order.Description;
        order.Status = request.Order.Status;
        order.UpdatedAt = DateTimeOffset.UtcNow;

        if (previousStatus != order.Status)
            await Publisher.Publish(
                new OrderStatusChangedDomainEvent(order.OrderNumber, previousStatus, order.Status, order.UpdatedAt),
                cancellationToken);

        await DatabaseContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
