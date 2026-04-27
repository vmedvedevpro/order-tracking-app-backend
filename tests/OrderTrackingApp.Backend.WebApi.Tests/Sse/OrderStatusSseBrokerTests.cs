using System.Collections;
using System.Reflection;

using OrderTrackingApp.Backend.Application.IntegrationEvents.Orders;
using OrderTrackingApp.Backend.WebApi.Sse;

namespace OrderTrackingApp.Backend.WebApi.Tests.Sse;

public class OrderStatusSseBrokerTests
{
    private static OrderStatusChangedIntegrationEvent NewEvent(long number) =>
        new() { OrderNumber = number, OldStatus = "Created", NewStatus = "Shipped", OccurredAt = DateTimeOffset.UtcNow };

    [Fact]
    public async Task NotifyAsync_WithoutSubscribers_ShouldNotThrow()
    {
        var sut = new OrderStatusSseBroker();

        var act = async () => await sut.NotifyAsync(NewEvent(1), CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SubscribeAsync_ShouldReceiveSubsequentNotifications()
    {
        var sut = new OrderStatusSseBroker();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var received = new List<OrderStatusChangedIntegrationEvent>();

        var consumer = Task.Run(
            async () =>
            {
                await foreach (var e in sut.SubscribeAsync(cts.Token))
                {
                    received.Add(e);
                    if (received.Count == 2) break;
                }
            },
            cts.Token);

        // Give the consumer a moment to register the subscription
        await WaitForSubscribersAsync(sut, expected: 1, cts.Token);

        await sut.NotifyAsync(NewEvent(1), CancellationToken.None);
        await sut.NotifyAsync(NewEvent(2), CancellationToken.None);

        await consumer;

        received.Select(e => e.OrderNumber).Should().Equal(1, 2);
    }

    [Fact]
    public async Task NotifyAsync_ShouldFanOutToAllSubscribers()
    {
        var sut = new OrderStatusSseBroker();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        async Task<OrderStatusChangedIntegrationEvent> ReadOneAsync()
        {
            await foreach (var e in sut.SubscribeAsync(cts.Token))
                return e;

            throw new InvalidOperationException("No event received");
        }

        var task1 = Task.Run(ReadOneAsync, cts.Token);
        var task2 = Task.Run(ReadOneAsync, cts.Token);

        await WaitForSubscribersAsync(sut, expected: 2, cts.Token);

        await sut.NotifyAsync(NewEvent(42), CancellationToken.None);

        var results = await Task.WhenAll(task1, task2);
        results.Should().OnlyContain(e => e.OrderNumber == 42);
    }

    [Fact]
    public async Task SubscribeAsync_AfterCancellation_ShouldRemoveSubscriber()
    {
        var sut = new OrderStatusSseBroker();
        var cts = new CancellationTokenSource();

        var consumer = Task.Run(async () =>
                                {
                                    try
                                    {
                                        await foreach (var _ in sut.SubscribeAsync(cts.Token))
                                        {
                                        }
                                    }
                                    catch (OperationCanceledException)
                                    {
                                    }
                                });

        await WaitForSubscribersAsync(sut, expected: 1, CancellationToken.None);

        await cts.CancelAsync();
        await consumer;

        // After cancellation the writer must be unregistered.
        GetSubscriberCount(sut).Should().Be(0);
        cts.Dispose();
    }

    private static async Task WaitForSubscribersAsync(OrderStatusSseBroker broker, int expected, CancellationToken cancellationToken)
    {
        while (GetSubscriberCount(broker) < expected)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(20, cancellationToken);
        }
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
