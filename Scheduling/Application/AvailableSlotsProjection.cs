using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.Infrastructure.Projections;

namespace Scheduling.Application
{
    public class AvailableSlotsProjection : EventHandler
    {
        public AvailableSlotsProjection(IAvailableSlotsRepository availableSlotsRepository)
        {
            When<SlotScheduled>((e, m) =>
                availableSlotsRepository.AddSlot(new AvailableSlot(
                    e.SlotId.ToString(),
                    e.DayId,
                    e.SlotStartTime.Date,
                    e.SlotStartTime,
                    e.SlotDuration,
                    false
                )));

            When<SlotBooked>((e, m) =>
                availableSlotsRepository.HideSlot(e.SlotId));

            When<SlotBookingCancelled>((e, m) =>
                availableSlotsRepository.ShowSlot(e.SlotId));

            When<SlotScheduleCancelled>((e, m) =>
                availableSlotsRepository.DeleteSlot(e.SlotId));
        }
    }
}
