using MediatR;

using OrderTrackingApp.Backend.Application.Common.Models;
using OrderTrackingApp.Backend.Application.Orders.Command.Create;
using OrderTrackingApp.Backend.Application.Orders.Command.Update;
using OrderTrackingApp.Backend.Application.Orders.Queries.Get;
using OrderTrackingApp.Backend.Application.Orders.Queries.GetById;
using OrderTrackingApp.Backend.Domain.Entities;
using OrderTrackingApp.Backend.WebApi.Endpoints.Extensions;

namespace OrderTrackingApp.Backend.WebApi.Endpoints;

public static class OrdersEndpoints
{
    public static RouteGroupBuilder MapOrders(this RouteGroupBuilder group) =>
        group.MapGroup("orders", MapEndpoints);

    private static void MapEndpoints(RouteGroupBuilder g)
    {
        g.MapPost(
             "",
             async (CreateOrderCommand request, ISender sender, CancellationToken cancellationToken) =>
                 await sender.Send(request, cancellationToken)
             )
         .WithSummary("Creates order")
         .Produces<Order>()
         .ProducesValidationProblem()
         .ProducesProblem(500);

        g.MapPut(
             "",
             async (UpdateOrderCommand request, ISender sender, CancellationToken cancellationToken) =>
                 await sender.Send(request, cancellationToken)
             )
         .WithSummary("Updates order")
         .Produces<Order>()
         .ProducesValidationProblem()
         .ProducesProblem(500);

        g.MapGet(
             "{id:long}",
             async ([AsParameters] GetOrderByIdQuery request, ISender sender, CancellationToken cancellationToken) =>
                 await sender.Send(request, cancellationToken)
             )
         .WithSummary("Get order by order number")
         .Produces<Order>()
         .ProducesProblem(400)
         .ProducesProblem(404)
         .ProducesProblem(500);

        g.MapGet(
             "",
             async ([AsParameters] GetOrdersQuery request, ISender sender, CancellationToken cancellationToken) =>
                 await sender.Send(request, cancellationToken)
             )
         .WithSummary("Get order by order number")
         .Produces<PagedResult<Order>>()
         .ProducesProblem(400)
         .ProducesProblem(500);
    }
}
