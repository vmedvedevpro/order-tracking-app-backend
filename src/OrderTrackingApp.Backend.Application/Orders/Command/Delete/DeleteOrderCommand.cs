using MediatR;

namespace OrderTrackingApp.Backend.Application.Orders.Command.Delete;

public record DeleteOrderCommand(long Id) : IRequest;
