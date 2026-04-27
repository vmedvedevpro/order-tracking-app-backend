using OrderTrackingApp.Backend.Application.Common.Exceptions;
using OrderTrackingApp.Backend.Application.Orders.Command.Delete;
using OrderTrackingApp.Backend.Application.Tests.TestHelpers;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Command.Delete;

public class DeleteOrderHandlerTests
{
    [Fact]
    public async Task Handle_WhenOrderExists_ShouldRemoveOrder()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var existing = new Order { Description = "d" };
        db.Orders.Add(existing);
        await db.SaveChangesAsync(CancellationToken.None);
        var sut = new DeleteOrderHandler(db);

        await sut.Handle(new DeleteOrderCommand(existing.OrderNumber), CancellationToken.None);

        db.Orders.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var sut = new DeleteOrderHandler(db);

        var act = () => sut.Handle(new DeleteOrderCommand(123), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("*123*");
    }
}
