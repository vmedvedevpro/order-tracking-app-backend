using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Infrastructure.Persistence;

public partial class DatabaseContext
{
    public DbSet<Order> Orders { get; protected set; }
}
