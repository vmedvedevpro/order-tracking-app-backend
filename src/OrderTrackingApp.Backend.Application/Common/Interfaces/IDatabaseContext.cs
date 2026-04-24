using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Common.Interfaces;

public interface IDatabaseContext
{
    DbSet<Order> Orders { get; }
}
