using System;
using Scheduling.Domain.Domain.DoctorDay.Commands;

namespace Scheduling.Controllers
{
    public class BookSlotRequest
    {
        public Guid SlotId { get; set; }

        public string PatientId { get; set; }

        public BookSlot ToCommand(string dayId)
        {
            return new BookSlot(dayId, SlotId, PatientId);
        }
    }
}
