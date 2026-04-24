using MediatR;

using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Application.Common.Exceptions;
using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Queries.GetById;

internal record GetOrderByIdHandler(IDatabaseContext DatabaseContext) : IRequestHandler<GetOrderByIdQuery, Order>
{
    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await DatabaseContext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderNumber == request.Id, cancellationToken);

        return order ?? throw new NotFoundException($"Order '{request.Id}' not found");
    }
}
