using System.Collections;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Http;

using OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;
using OrderTrackingApp.Backend.WebApi.Sse;

namespace OrderTrackingApp.Backend.WebApi.Tests.Sse;

public class OrderStatusSseWriterTests
{
    [Fact]
    public async Task WriteAsync_ShouldSetSseHeaders()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        var broker = new OrderStatusSseBroker();
        using var cts = new CancellationTokenSource();

        var writeTask = OrderStatusSseWriter.WriteAsync(httpContext, broker, cts.Token);

        // Headers are written synchronously before the first await on the channel.
        await Task.Delay(50);
        httpContext.Response.Headers.ContentType.ToString().Should().Be("text/event-stream");
        httpContext.Response.Headers.CacheControl.ToString().Should().Be("no-cache");

        await cts.CancelAsync();
        try
        {
            await writeTask;
        }
        catch (OperationCanceledException)
        {
        }
    }

    [Fact]
    public async Task WriteAsync_WhenEventPublished_ShouldStreamSseFrame()
    {
        var httpContext = new DefaultHttpContext();
        var body = new MemoryStream();
        httpContext.Response.Body = body;
        var broker = new OrderStatusSseBroker();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var writeTask = OrderStatusSseWriter.WriteAsync(httpContext, broker, cts.Token);

        // Wait until subscriber registered.
        for (var i = 0; i < 50 && GetSubscriberCount(broker) == 0; i++)
            await Task.Delay(20);

        await broker.NotifyAsync(
            new OrderStatusChangedIntegrationEvent
            {
                OrderNumber = 99, OldStatus = "Created", NewStatus = "Shipped", OccurredAt = DateTimeOffset.UtcNow
            },
            CancellationToken.None);

        // Wait until something is written.
        for (var i = 0; i < 50 && body.Length == 0; i++)
            await Task.Delay(20);

        await cts.CancelAsync();
        try
        {
            await writeTask;
        }
        catch (OperationCanceledException)
        {
        }

        var text = Encoding.UTF8.GetString(body.ToArray());
        text.Should().Contain("event: order-status-changed");
        text.Should().Contain("\"orderNumber\":99");
        text.Should().Contain("\"newStatus\":\"Shipped\"");
    }

    private static int GetSubscriberCount(OrderStatusSseBroker broker)
    {
        var field = typeof(OrderStatusSseBroker).GetField(
            "_subscribers",
            BindingFlags.Instance | BindingFlags.NonPublic);
        var dict = (ICollection)field!.GetValue(broker)!;
        return dict.Count;
    }
}
