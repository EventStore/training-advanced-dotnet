using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public record DayScheduleCancelled(
        string DayId
    ) : IEvent;
}
