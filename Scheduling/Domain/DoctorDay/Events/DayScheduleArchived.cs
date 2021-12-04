using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events;

public record DayScheduleArchived(
    string DayId
) : IEvent;