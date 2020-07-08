using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public class ArchiveDaySchedule : Command<ArchiveDaySchedule>
    {
        public string DayId { get; }

        public ArchiveDaySchedule(string dayId)
        {
            DayId = dayId;
        }
    }
}
