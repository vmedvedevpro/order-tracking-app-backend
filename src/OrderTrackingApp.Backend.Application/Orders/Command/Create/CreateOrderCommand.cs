using MediatR;

using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Create;

public record CreateOrderCommand(Order Order) : IRequest<Order>;
