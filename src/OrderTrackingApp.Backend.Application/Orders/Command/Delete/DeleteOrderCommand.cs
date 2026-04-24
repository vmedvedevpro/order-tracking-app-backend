using MediatR;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Delete;

public record DeleteOrderCommand(ulong Id) : IRequest;
