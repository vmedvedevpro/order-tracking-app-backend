using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Tests.TestHelpers;

internal sealed class TestDatabaseContext(DbContextOptions<TestDatabaseContext> options) : DbContext(options), IDatabaseContext
{
    public DbSet<Order> Orders => Set<Order>();

    Task<int> IDatabaseContext.SaveChangesAsync(CancellationToken cancellationToken) =>
        base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Order>(b =>
                                   {
                                       b.HasKey(o => o.OrderNumber);
                                       b.Property(o => o.OrderNumber).ValueGeneratedOnAdd();
                                   });

    public static TestDatabaseContext CreateInMemory()
    {
        var options = new DbContextOptionsBuilder<TestDatabaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
        return new TestDatabaseContext(options);
    }
}
