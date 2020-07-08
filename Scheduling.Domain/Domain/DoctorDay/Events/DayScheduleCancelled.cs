using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Events
{
    public class DayScheduleCancelled : Event<DayScheduleCancelled>
    {
        public string DayId { get; }

        public DayScheduleCancelled(string dayId)
        {
            DayId = dayId;
        }
    }
}
