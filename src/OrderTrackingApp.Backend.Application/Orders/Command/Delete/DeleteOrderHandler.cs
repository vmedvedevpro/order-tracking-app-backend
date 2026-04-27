using MediatR;

using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Application.Common.Exceptions;
using OrderTrackingApp.Backend.Application.Common.Interfaces;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Delete;

internal record DeleteOrderHandler(IDatabaseContext DatabaseContext) : IRequestHandler<DeleteOrderCommand>
{
    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await DatabaseContext.Orders.FirstOrDefaultAsync(x => x.OrderNumber == request.Id, cancellationToken);

        if (order == null)
            throw new NotFoundException($"Order '{request.Id}' not found");

        DatabaseContext.Orders.Remove(order);
        await DatabaseContext.SaveChangesAsync(cancellationToken);
    }
}
