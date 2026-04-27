using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

using OrderTrackingApp.Backend.Application.Common.Interfaces;
using OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;

namespace OrderTrackingApp.Backend.WebApi.Sse;

internal sealed class OrderStatusSseBroker : IOrderStatusNotifier
{
    private static readonly BoundedChannelOptions ChannelOptions = new(capacity: 256)
                                                                   {
                                                                       SingleReader = true,
                                                                       SingleWriter = false,
                                                                       FullMode = BoundedChannelFullMode.DropOldest
                                                                   };

    private readonly ConcurrentDictionary<ChannelWriter<OrderStatusChangedIntegrationEvent>, byte> _subscribers = new();

    public async IAsyncEnumerable<OrderStatusChangedIntegrationEvent> SubscribeAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var channel = Channel.CreateBounded<OrderStatusChangedIntegrationEvent>(ChannelOptions);
        _subscribers.TryAdd(channel.Writer, 0);

        try
        {
            await foreach (var statusChanged in channel.Reader.ReadAllAsync(cancellationToken))
            {
                yield return statusChanged;
            }
        }
        finally
        {
            _subscribers.TryRemove(channel.Writer, out _);
            channel.Writer.TryComplete();
        }
    }

    public ValueTask NotifyAsync(
        OrderStatusChangedIntegrationEvent statusChanged,
        CancellationToken cancellationToken = default)
    {
        foreach (var (writer, _) in _subscribers)
        {
            writer.TryWrite(statusChanged);
        }

        return ValueTask.CompletedTask;
    }
}
