using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public record CancelDaySchedule(
        string DayId
    ) : ICommand;
}
