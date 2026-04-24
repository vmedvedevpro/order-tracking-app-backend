using MediatR;

using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Application.Common.Exceptions;
using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Update;

internal record UpdateOrderHandler(IDatabaseContext DatabaseContext) : IRequestHandler<UpdateOrderCommand, Order>
{
    public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await DatabaseContext.Orders.FirstOrDefaultAsync(x => x.OrderNumber == request.Order.OrderNumber, cancellationToken);

        if (order == null)
            throw new NotFoundException($"Order '{request.Order.OrderNumber}' not found");

        order.Description = request.Order.Description;
        order.Status = request.Order.Status;
        order.UpdatedAt = DateTimeOffset.UtcNow;

        await DatabaseContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
