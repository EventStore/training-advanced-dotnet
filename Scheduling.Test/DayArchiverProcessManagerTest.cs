using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EventStore.Client;
using Scheduling.Application;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.EventSourcing;
using Scheduling.Infrastructure.EventStore;
using Scheduling.Infrastructure.InMemory;
using Scheduling.Test.Test;
using Xunit;
using EventHandler = Scheduling.Infrastructure.Projections.EventHandler;

namespace Scheduling.Test
{
    public class DayArchiverProcessManagerTest : HandlerTest
    {
        private readonly DateTime _now = DateTime.UtcNow;

        private readonly IEventStore _esStore;

        private readonly TimeSpan _tenMinutes = TimeSpan.FromMinutes(10);

        private readonly InMemoryColdStorage _inMemoryColdStorage = new InMemoryColdStorage();

        private EsCommandStore _esCommandStore;

        public DayArchiverProcessManagerTest()
        {
            EventMappings.MapEventTypes();
            _esStore = new EsEventStore(GetEventStoreClient(), "test");
            _esCommandStore = new EsCommandStore(_esStore, null, null, null);
        }

        protected override EventHandler GetHandler()
        {
            return new DayArchiverProcessManager(
                _inMemoryColdStorage,
                new InMemoryArchivableDaysRepository(),
                _esCommandStore,
                TimeSpan.FromDays(-1),
                _esStore,
                Guid.NewGuid
            );
        }

        [Fact]
        public async Task should_archive_all_events_and_truncate_all_except_last_one()
        {
            var dayId = Guid.NewGuid().ToString();
            var scheduled = new SlotScheduled(Guid.NewGuid(), dayId, _now, _tenMinutes);
            var slotBooked = new SlotBooked(dayId, scheduled.SlotId, "PatientId");
            var dayArchived = new DayScheduleArchived(dayId);

            var metadata = new CommandMetadata(new CorrelationId(Guid.NewGuid()), new CausationId(Guid.NewGuid()));

            var events = new List<object> {scheduled, slotBooked, dayArchived};

            await _esStore.AppendEvents("Day-" + dayId, metadata, events.ToArray());
            await Given(dayArchived);
            Then(events, _inMemoryColdStorage.Events);
            Then(new List<object> {dayArchived}, await _esStore.LoadEvents("Day-" + dayId));
        }

        [Fact]
        public async Task should_send_archive_command_for_all_slots_completed_180_days_ago()
        {
            var dayId = Guid.NewGuid().ToString();
            var date = DateTime.UtcNow.AddDays(-180);
            var dayScheduled = new DayScheduled(dayId, Guid.NewGuid(), date);
            var calendarDayStarted = new CalendarDayStarted(_now);

            await Given(dayScheduled, calendarDayStarted);
            Then(
                new ArchiveDaySchedule(dayId),
                (await _esStore.LoadCommands("async_command_handler-day")).Last().Command);
        }

        private static EventStoreClient GetEventStoreClient() =>
            new EventStoreClient(new EventStoreClientSettings
            {
                ConnectivitySettings =
                {
                    Address = new Uri("http://localhost:2113"),
                },
                DefaultCredentials = new UserCredentials("admin", "changeit")
            });
    }
}
