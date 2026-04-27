using MediatR;

using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Create;

public record CreateOrderCommand(string Description) : IRequest<Order>;
