using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public class CancelDaySchedule : Command<BookSlot>
    {
        public string DayId { get; }

        public CancelDaySchedule(string dayId)
        {
            DayId = dayId;
        }
    }
}
