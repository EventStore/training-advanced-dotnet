using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
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
