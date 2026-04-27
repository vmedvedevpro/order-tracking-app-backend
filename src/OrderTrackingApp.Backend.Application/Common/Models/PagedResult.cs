namespace OrderTrackingApp.Backend.Application.Common.Models;

public class PagedResult<T>
{
    public ICollection<T> Items { get; set; } = [];

    public int Count { get; set; }

    public int PageNumber { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages { get; set; }
}
