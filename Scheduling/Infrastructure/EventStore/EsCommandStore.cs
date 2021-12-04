using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Scheduling.EventSourcing;
using Scheduling.Infrastructure.Commands;

namespace Scheduling.Infrastructure.EventStore;

public class EsCommandStore : ICommandStore
{
    private readonly IEventStore _eventStore;
    private readonly EventStoreClient _client;
    private readonly Dispatcher _dispatcher;
    private readonly string _tenantPrefix;
    private string _streamName = "async_command_handler-day";

    public EsCommandStore(IEventStore eventStore, EventStoreClient client, Dispatcher dispatcher,
        string tenantPrefix)
    {
        _eventStore = eventStore;
        _client = client;
        _dispatcher = dispatcher;
        _tenantPrefix = $"[{tenantPrefix}]";
    }

    public Task Send(object command, CommandMetadata metadata)
    {
        return _eventStore.AppendCommand(_streamName, command, metadata);
    }

    public async Task Start()
    {
        await _client.SubscribeToStreamAsync(_tenantPrefix + _streamName, StreamPosition.End, EventAppeared,
            subscriptionDropped: SubscriptionDropped);
    }

    private void SubscriptionDropped(StreamSubscription _, SubscriptionDroppedReason r, Exception? e)
    {
        Console.WriteLine(e);
    }

    private async Task EventAppeared(StreamSubscription _, ResolvedEvent @event, CancellationToken c)
    {
        var commandEnvelope = @event.DeserializeCommand();

        try
        {
            await _dispatcher.Dispatch(commandEnvelope.command, commandEnvelope.metadata);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}