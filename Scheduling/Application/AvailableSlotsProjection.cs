using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.Infrastructure.Projections;

namespace Scheduling.Application
{
    public class AvailableSlotsProjection : EventHandler
    {
        public AvailableSlotsProjection(IAvailableSlotsRepository availableSlotsRepository)
        {
            When<SlotScheduled>(e =>
                availableSlotsRepository.AddSlot(new AvailableSlot
                {
                    Id = e.SlotId.ToString(),
                    DayId = e.DayId,
                    Duration = e.SlotDuration,
                    Date = e.SlotStartTime.Date,
                    StartTime = e.SlotStartTime,
                    IsBooked = false
                }));

            When<SlotBooked>(e =>
                availableSlotsRepository.HideSlot(e.SlotId));

            When<SlotBookingCancelled>(e =>
                availableSlotsRepository.ShowSlot(e.SlotId));

            When<SlotScheduleCancelled>(e =>
                availableSlotsRepository.DeleteSlot(e.SlotId));
        }
    }
}
