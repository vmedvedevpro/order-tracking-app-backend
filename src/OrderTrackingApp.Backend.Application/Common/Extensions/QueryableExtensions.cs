using Microsoft.EntityFrameworkCore;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Application.Common.Models;

namespace OrderTrackingApp.Backend.Application.Common.Extensions;

public static class QueryableExtensions
{
    private const int DefaultPageSize = 10;

    private const int DefaultPageNumber = 1;

    public static async Task<PagedResult<T>> ToPagedResult<T>(
        this IQueryable<T> queryable,
        IPagedQuery request,
        CancellationToken cancellationToken)
    {
        var totalCount = await queryable.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)(request.PageSize ?? DefaultPageSize));
        var items = await queryable
                          .Skip(request.PageNumber ?? DefaultPageNumber * request.PageSize ?? DefaultPageSize)
                          .Take(request.PageSize ?? DefaultPageSize)
                          .ToArrayAsync(cancellationToken);

        return new PagedResult<T>
               {
                   Count = items.Length,
                   PageNumber = request.PageNumber ?? DefaultPageNumber,
                   TotalItems = totalCount,
                   Items = items,
                   TotalPages = totalPages
               };
    }
}
