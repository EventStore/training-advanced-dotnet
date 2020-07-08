using System;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Commands
{
    public class BookSlot : Command<BookSlot>
    {
        public Guid SlotId { get; }

        public string PatientId { get; }

        public string DayId { get; }

        public BookSlot(string dayId, Guid slotId, string patientId)
        {
            DayId = dayId;
            SlotId = slotId;
            PatientId = patientId;
        }
    }
}
