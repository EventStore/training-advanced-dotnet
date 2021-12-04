using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EventStore.Client;
using Scheduling.Domain.DoctorDay;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.EventSourcing;
using Scheduling.Infrastructure.EventStore;
using Xunit;

namespace Scheduling.Test;

[Collection("TypeMapper collection")]
public class SnapshotTest
{
    [Fact]
    public async Task write_snapshot_if_threshold_reached()
    {
        EventMappings.MapEventTypes();

        var now = DateTime.UtcNow;
        var esStore = new EsEventStore(GetEventStoreClient(), "snapshot_test");
        var esAggregateStore = new EsAggregateStore(esStore, 5);

        var aggregate = new Day();

        var slots = new List<ScheduledSlot>
        {
            new ScheduledSlot(TimeSpan.FromMinutes(10), now),
            new ScheduledSlot(TimeSpan.FromMinutes(10), now.AddMinutes(10)),
            new ScheduledSlot(TimeSpan.FromMinutes(10), now.AddMinutes(20)),
            new ScheduledSlot(TimeSpan.FromMinutes(10), now.AddMinutes(30)),
            new ScheduledSlot(TimeSpan.FromMinutes(10), now.AddMinutes(40))
        };

        aggregate.Schedule(new DoctorId(Guid.NewGuid()), DateTime.UtcNow, slots, Guid.NewGuid);

        await esAggregateStore.Save(aggregate,
            new CommandMetadata(new CorrelationId(Guid.NewGuid()), new CausationId(Guid.NewGuid())));

        var snapshotEnvelope = await esStore.LoadSnapshot(StreamName.For<Day>(aggregate.Id));
        var snapshot = snapshotEnvelope?.Snapshot as DaySnapshot;

        Assert.NotNull(snapshot);
    }

    [Fact]
    public async Task read_snapshot_when_loading_aggregate()
    {
        EventMappings.MapEventTypes();

        var now = DateTime.UtcNow;
        var esStore = new EsEventStore(GetEventStoreClient(), "snapshot_test");
        var esAggregateStore = new EsAggregateStore(esStore, 5);

        var aggregate = new Day();

        var slots = new List<ScheduledSlot>
        {
            new ScheduledSlot(TimeSpan.FromMinutes(10), now),
            new ScheduledSlot(TimeSpan.FromMinutes(10), now.AddMinutes(10)),
            new ScheduledSlot(TimeSpan.FromMinutes(10), now.AddMinutes(20)),
            new ScheduledSlot(TimeSpan.FromMinutes(10), now.AddMinutes(30)),
            new ScheduledSlot(TimeSpan.FromMinutes(10), now.AddMinutes(40))
        };

        aggregate.Schedule(new DoctorId(Guid.NewGuid()), DateTime.UtcNow, slots, Guid.NewGuid);

        await esAggregateStore.Save(aggregate,
            new CommandMetadata(new CorrelationId(Guid.NewGuid()), new CausationId(Guid.NewGuid())));

        await esStore.TruncateStream(StreamName.For<Day>(aggregate.Id),
            Convert.ToUInt64(aggregate.GetChanges().Count()));

        var reloadedAggregate = await esAggregateStore.Load<Day>(aggregate.Id);

        Assert.Equal(5, reloadedAggregate.Version);
    }

    public static EventStoreClient GetEventStoreClient() =>
        new(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false"));
}