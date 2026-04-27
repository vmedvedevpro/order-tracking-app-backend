using MediatR;

using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Update;

public record UpdateOrderCommand(Order Order) : IRequest<Order>;
