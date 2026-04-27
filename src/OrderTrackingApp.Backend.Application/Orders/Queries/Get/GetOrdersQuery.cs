using MediatR;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Application.Common.Models;
using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Application.Orders.Queries.Get;

public record GetOrdersQuery(int? PageNumber, int? PageSize) : IRequest<PagedResult<Order>>, IPagedQuery;
