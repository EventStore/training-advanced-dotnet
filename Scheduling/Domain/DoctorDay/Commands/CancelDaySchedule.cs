using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public class CancelDaySchedule : Command<CancelDaySchedule>
    {
        public string DayId { get; }

        public CancelDaySchedule(string dayId)
        {
            DayId = dayId;
        }
    }
}
