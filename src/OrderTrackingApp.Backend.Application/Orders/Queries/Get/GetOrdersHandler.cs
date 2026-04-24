using MediatR;

using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Application.Common.Extensions;
using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Application.Common.Models;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Queries.Get;

internal record GetOrdersHandler(IDatabaseContext DatabaseContext) : IRequestHandler<GetOrdersQuery, PagedResult<Order>>
{
    public async Task<PagedResult<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken) =>
        await DatabaseContext.Orders.AsNoTracking().ToPagedResult(request, cancellationToken);
}
