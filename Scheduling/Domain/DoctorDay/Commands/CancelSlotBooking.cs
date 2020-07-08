using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public class CancelSlotBooking : Command<CancelSlotBooking>
    {
        public string DayId { get; }

        public Guid SlotId { get; }

        public string Reason { get; }

        public CancelSlotBooking(string dayId, Guid slotId, string reason)
        {
            DayId = dayId;
            SlotId = slotId;
            Reason = reason;
        }
    }
}
