using MediatR;

using OrderTrackingApp.Backend.Application.Common.Exceptions;
using OrderTrackingApp.Backend.Application.Orders.Command.Update;
using OrderTrackingApp.Backend.Application.Orders.Events;
using OrderTrackingApp.Backend.Application.Tests.TestHelpers;
using OrderTrackingApp.Backend.Domain.Entities;
using OrderTrackingApp.Backend.Domain.Enums;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Command.Update;

public class UpdateOrderHandlerTests
{
    private readonly IPublisher _publisher = Substitute.For<IPublisher>();

    [Fact]
    public async Task Handle_WhenOrderExistsAndStatusUnchanged_ShouldUpdateAndNotPublish()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var existing = new Order
                       {
                           Description = "old",
                           Status = OrderStatus.Created,
                           CreatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                           UpdatedAt = DateTimeOffset.UtcNow.AddDays(-1)
                       };
        db.Orders.Add(existing);
        await db.SaveChangesAsync(CancellationToken.None);

        var sut = new UpdateOrderHandler(db, _publisher);
        var updated = new Order { OrderNumber = existing.OrderNumber, Description = "new", Status = OrderStatus.Created };

        var result = await sut.Handle(new UpdateOrderCommand(updated), CancellationToken.None);

        result.Description.Should().Be("new");
        result.Status.Should().Be(OrderStatus.Created);
        await _publisher.DidNotReceive()
                        .Publish(Arg.Any<OrderStatusChangedDomainEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenStatusChanged_ShouldPublishDomainEvent()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var existing = new Order { Description = "d", Status = OrderStatus.Created };
        db.Orders.Add(existing);
        await db.SaveChangesAsync(CancellationToken.None);

        var sut = new UpdateOrderHandler(db, _publisher);
        var updated = new Order { OrderNumber = existing.OrderNumber, Description = "d", Status = OrderStatus.Shipped };

        await sut.Handle(new UpdateOrderCommand(updated), CancellationToken.None);

        await _publisher.Received(1).Publish(
            Arg.Is<OrderStatusChangedDomainEvent>(e =>
                                                      e.OrderNumber == existing.OrderNumber
                                                      && e.OldStatus == OrderStatus.Created
                                                      && e.NewStatus == OrderStatus.Shipped),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var sut = new UpdateOrderHandler(db, _publisher);
        var command = new UpdateOrderCommand(new Order { OrderNumber = 42, Description = "x", Status = OrderStatus.Created });

        var act = () => sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("*42*");
        await _publisher.DidNotReceive().Publish(Arg.Any<OrderStatusChangedDomainEvent>(), Arg.Any<CancellationToken>());
    }
}
