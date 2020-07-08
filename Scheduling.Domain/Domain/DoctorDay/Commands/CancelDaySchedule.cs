using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Commands
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
