using System.Buffers;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;

using OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;

namespace OrderTrackingApp.Backend.WebApi.Sse;

internal static class OrderStatusSseWriter
{
    private const string OrderStatusChangedEventType = "order-status-changed";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public static Task WriteAsync(
        HttpContext httpContext,
        OrderStatusSseBroker broker,
        CancellationToken cancellationToken)
    {
        httpContext.Response.Headers.ContentType = "text/event-stream";
        httpContext.Response.Headers.CacheControl = "no-cache";

        return SseFormatter.WriteAsync(
            ToSseItems(broker.SubscribeAsync(cancellationToken), cancellationToken),
            httpContext.Response.Body,
            FormatEvent,
            cancellationToken);
    }

    private static async IAsyncEnumerable<SseItem<OrderStatusChangedIntegrationEvent>> ToSseItems(
        IAsyncEnumerable<OrderStatusChangedIntegrationEvent> source,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var statusChanged in source.WithCancellation(cancellationToken))
        {
            yield return new SseItem<OrderStatusChangedIntegrationEvent>(statusChanged, OrderStatusChangedEventType);
        }
    }

    private static void FormatEvent(SseItem<OrderStatusChangedIntegrationEvent> item, IBufferWriter<byte> writer)
    {
        using var json = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(json, item.Data, JsonSerializerOptions);
    }
}
