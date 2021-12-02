using System;
using System.Threading.Tasks;
using Scheduling.Domain.DoctorDay;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.EventSourcing;
using Scheduling.Infrastructure.EventStore;

namespace Scheduling.Application;

public class DayArchiverProcessManager : Infrastructure.Projections.EventHandler
{
    private readonly ICommandStore _commandStore;
    private readonly Func<Guid> _idGenerator;

    public DayArchiverProcessManager(
        IColdStorage coldStorage,
        IArchivableDaysRepository archivableDaysRepository,
        ICommandStore commandStore,
        TimeSpan threshold,
        IEventStore eventStore,
        Func<Guid> idGenerator
    )
    {
        _commandStore = commandStore;
        _idGenerator = idGenerator;

        When<DayScheduled>(async (e,m) =>
        {
            await archivableDaysRepository.Add(new ArchivableDay(e.DayId, e.Date));
        });

        When<CalendarDayStarted>(async (e,m) =>
        {
            var archivableDays = await archivableDaysRepository.FindAll(e.Date.Add(threshold));
            foreach (var day in archivableDays)
            {
                await SendCommand(day.Id, m.CorrelationId, m.CausationId);
            }
        });

        When<DayScheduleArchived>(async (e,m) =>
        {
            var streamName = StreamName.For<Day>(e.DayId);

            var events = await eventStore.LoadEvents(streamName);
            coldStorage.SaveAll(events);

            var lastVersion = await eventStore.GetLastVersion(streamName);

            if (lastVersion.HasValue)
            {
                await eventStore.TruncateStream(streamName, lastVersion.Value);
            }
        });
    }

    private async Task SendCommand(string dayId, CorrelationId correlationId, CausationId causationId)
    {
        await _commandStore.Send(new ArchiveDaySchedule(dayId), new CommandMetadata(correlationId, causationId));
    }
}