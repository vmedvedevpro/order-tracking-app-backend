using OrderTrackingApp.Backend.Application.Common.Exceptions;
using OrderTrackingApp.Backend.Application.Orders.Queries.GetById;
using OrderTrackingApp.Backend.Application.Tests.TestHelpers;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Queries.GetById;

public class GetOrderByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenOrderExists_ShouldReturnOrder()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var existing = new Order { Description = "abc" };
        db.Orders.Add(existing);
        await db.SaveChangesAsync(CancellationToken.None);
        var sut = new GetOrderByIdHandler(db);

        var result = await sut.Handle(new GetOrderByIdQuery(existing.OrderNumber), CancellationToken.None);

        result.Should().NotBeNull();
        result.OrderNumber.Should().Be(existing.OrderNumber);
        result.Description.Should().Be("abc");
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var sut = new GetOrderByIdHandler(db);

        var act = () => sut.Handle(new GetOrderByIdQuery(404), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("*404*");
    }
}
