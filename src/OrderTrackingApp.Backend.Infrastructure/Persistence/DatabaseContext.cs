using MassTransit;

using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Application.Common.Interfaces;

namespace OrderTrackingApp.Backend.Infrastructure.Persistence;

public partial class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IDatabaseContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

        modelBuilder.AddTransactionalOutboxEntities();
    }
}
