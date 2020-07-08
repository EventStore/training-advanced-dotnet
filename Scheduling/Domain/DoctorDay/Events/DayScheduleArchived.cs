using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public class DayScheduleArchived : Event<DayScheduleArchived>
    {
        public string DayId { get; }

        public DayScheduleArchived(string dayId)
        {
            DayId = dayId;
        }
    }
}
