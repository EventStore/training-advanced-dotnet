using System;
using Scheduling.Domain.DoctorDay.Commands;

namespace Scheduling.Controllers
{
    public class CancelSlotBookingRequest
    {
        public Guid SlotId { get; set; }

        public string Reason { get; set; }

        public CancelSlotBooking ToCommand(string dayId)
        {
            return new CancelSlotBooking(dayId, SlotId, Reason);
        }
    }
}