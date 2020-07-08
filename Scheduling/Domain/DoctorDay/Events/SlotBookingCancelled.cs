using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public class SlotBookingCancelled : Event<SlotBookingCancelled>
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
