using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public record CancelSlotBooking(
        string DayId,
        Guid SlotId,
        string Reason
    ) : ICommand;
}
