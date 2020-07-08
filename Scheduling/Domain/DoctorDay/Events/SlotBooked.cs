using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public class SlotBooked : Event<SlotBooked>
    {
        public string DayId { get; }

        public Guid SlotId { get; }

        public string PatientId { get; }

        public SlotBooked(string dayId, Guid slotId, string patientId)
        {
            DayId = dayId;
            SlotId = slotId;
            PatientId = patientId;
        }
    }
}
