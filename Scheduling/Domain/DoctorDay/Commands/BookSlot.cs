using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
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
