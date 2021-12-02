using System;
using Scheduling.Domain.DoctorDay.Commands;

namespace Scheduling.Controllers
{
    public record BookSlotRequest(
        Guid SlotId,
        string PatientId
    )
    {
        public BookSlot ToCommand(string dayId) =>
            new BookSlot(dayId, SlotId, PatientId);
    }
}
