using MediatR;

using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Queries.GetById;

public record GetOrderByIdQuery(ulong Id) : IRequest<Order>;
