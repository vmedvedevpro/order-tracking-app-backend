using OrderTrackingApp.Backend.Application.Orders.Command.Create;
using OrderTrackingApp.Backend.Application.Tests.TestHelpers;
using OrderTrackingApp.Backend.Domain.Enums;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Command.Create;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPersistOrderAndReturnIt()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var sut = new CreateOrderHandler(db);
        var command = new CreateOrderCommand("New order");

        var result = await sut.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Description.Should().Be("New order");
        result.Status.Should().Be(OrderStatus.Created);
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        result.UpdatedAt.Should().BeCloseTo(result.CreatedAt, TimeSpan.FromMilliseconds(1));

        db.Orders.Should().ContainSingle().Which.Description.Should().Be("New order");
    }
}
