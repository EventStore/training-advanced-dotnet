using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Commands
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
