using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public record BookSlot(
        string DayId,
        Guid SlotId,
        string PatientId
    ) : ICommand;
}
