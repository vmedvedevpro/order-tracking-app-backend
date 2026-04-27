using OrderTrackingApp.Backend.Application.Orders.Queries.Get;
using OrderTrackingApp.Backend.Application.Tests.TestHelpers;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Tests.Orders.Queries.Get;

public class GetOrdersHandlerTests
{
    private static async Task SeedAsync(TestDatabaseContext db, int count)
    {
        for (var i = 0; i < count; i++)
            db.Orders.Add(new Order { Description = $"d{i}" });
        await db.SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_WithDefaultPaging_ShouldReturnFirstPageOfTen()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        await SeedAsync(db, 25);
        var sut = new GetOrdersHandler(db);

        var result = await sut.Handle(new GetOrdersQuery(null, null), CancellationToken.None);

        result.TotalItems.Should().Be(25);
        result.PageNumber.Should().Be(1);
        result.TotalPages.Should().Be(3);
        result.Items.Should().HaveCount(10);
        result.Count.Should().Be(10);
    }

    [Fact]
    public async Task Handle_WithExplicitPaging_ShouldReturnRequestedPage()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        await SeedAsync(db, 12);
        var sut = new GetOrdersHandler(db);

        var result = await sut.Handle(new GetOrdersQuery(2, 5), CancellationToken.None);

        result.TotalItems.Should().Be(12);
        result.PageNumber.Should().Be(2);
        result.TotalPages.Should().Be(3);
        result.Items.Should().HaveCount(5);
    }

    [Fact]
    public async Task Handle_WhenNoOrders_ShouldReturnEmptyResult()
    {
        await using var db = TestDatabaseContext.CreateInMemory();
        var sut = new GetOrdersHandler(db);

        var result = await sut.Handle(new GetOrdersQuery(1, 10), CancellationToken.None);

        result.TotalItems.Should().Be(0);
        result.Items.Should().BeEmpty();
        result.TotalPages.Should().Be(0);
    }
}
