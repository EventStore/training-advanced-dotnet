using System;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Events
{
    public class SlotBookingCancelled : Event<SlotBooked>
    {
        public string DayId { get; }

        public Guid SlotId { get; }

        public string Reason { get; }

        public SlotBookingCancelled(string dayId, Guid slotId, string reason)
        {
            DayId = dayId;
            SlotId = slotId;
            Reason = reason;
        }
    }
}
