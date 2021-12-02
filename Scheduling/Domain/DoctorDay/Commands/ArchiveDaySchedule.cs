using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands;

public record ArchiveDaySchedule(
    string DayId
) : ICommand;