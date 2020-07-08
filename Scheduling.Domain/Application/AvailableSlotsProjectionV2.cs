using Scheduling.Domain.Domain.DoctorDay.Events;
using Scheduling.Domain.Domain.ReadModel;
using Scheduling.Domain.Infrastructure.Projections;

namespace Scheduling.Domain.Application
{
    public class AvailableSlotsProjectionV2 : EventHandler
    {
        public AvailableSlotsProjectionV2(IAvailableSlotsRepository availableSlotsRepository)
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
