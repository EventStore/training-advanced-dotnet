using System;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.EventSourcing;

namespace Scheduling.Infrastructure.EventStore;

public class EsAggregateStore : IAggregateStore
{
    readonly IEventStore _store;
    private readonly int _snapshotThreshold;

    public EsAggregateStore(IEventStore store, int snapshotThreshold)
    {
        _store = store;
        _snapshotThreshold = snapshotThreshold;
    }

    public async Task Save<T>(T aggregate, CommandMetadata metadata) where T : AggregateRoot
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        var streamName = GetStreamName(aggregate);

        var changes = aggregate.GetChanges().ToArray();

        await _store.AppendEvents(streamName, aggregate.Version, metadata, changes);

        // Append snapshot
        if (aggregate is AggregateRootSnapshot snapshotAggregate)
        {
            if ((snapshotAggregate.Version + changes.Length + 1) - snapshotAggregate.SnapshotVersion >= _snapshotThreshold)
            {
                await _store.AppendSnapshot(streamName, aggregate.Version + changes.Length, snapshotAggregate.GetSnapshot());
            }
        }

        aggregate.ClearChanges();
    }

    public async Task<T> Load<T>(string aggregateId)
        where T : AggregateRoot
    {
        if (aggregateId == null)
            throw new ArgumentNullException(nameof(aggregateId));

        var streamName = GetStreamName<T>(aggregateId);
        var aggregate = (T) Activator.CreateInstance(typeof(T), true)!;

        aggregate.Id = aggregateId;

        var version = -1;

        // Load snapshot
        if (aggregate is AggregateRootSnapshot snapshotAggregate)
        {
            version = await LoadSnapshot(streamName, snapshotAggregate);
        }

        var events = await _store.LoadEvents(streamName, version);

        aggregate.Load(events);
        aggregate.ClearChanges();

        return aggregate;
    }

    private async Task<int> LoadSnapshot(string streamName, AggregateRootSnapshot snapshotAggregate)
    {
        var snapshotEnvelope = await _store.LoadSnapshot(streamName);

        if (snapshotEnvelope != null)
        {
            snapshotAggregate.LoadSnapshot(snapshotEnvelope.Snapshot, snapshotEnvelope.Metadata.Version);
            return snapshotEnvelope.Metadata.Version + 1;
        }

        return -1;
    }

    static string GetStreamName<T>(string aggregateId)
        where T : AggregateRoot
        => $"{typeof(T).Name}-{aggregateId}";

    static string GetStreamName<T>(T aggregate)
        where T : AggregateRoot
        => $"{typeof(T).Name}-{aggregate.Id:N}";
}