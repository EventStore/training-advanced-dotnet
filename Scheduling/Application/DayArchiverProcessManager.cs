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

        When<DayScheduled>(async e =>
        {
            await archivableDaysRepository.Add(new ArchivableDay(e.DayId, e.Date));
        });

        When<CalendarDayStarted>(async e =>
        {
            var archivableDays = await archivableDaysRepository.FindAll(e.Date.Add(threshold));
            foreach (var day in archivableDays)
            {
                await SendCommand(day.Id);
            }
        });

        When<DayScheduleArchived>(async e =>
        {
            var streamName = StreamName.For<Day>(e.DayId);

            // Get events from store
            // save to cold storage
            // Get the latest version from the store
            // Truncate stream upto last version
        });
    }

    private async Task SendCommand(string dayId)
    {
        await _commandStore.Send(new ArchiveDaySchedule(dayId), new CommandMetadata(new CorrelationId(_idGenerator()), new CausationId(_idGenerator())));
    }
}