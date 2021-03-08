using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.Infrastructure.Projections;

namespace Scheduling.Application
{
    public class AvailableSlotsProjectionV2 : EventHandler
    {
        public AvailableSlotsProjectionV2(IAvailableSlotsRepository availableSlotsRepository)
        {
            When<SlotScheduled>((e, m) =>
                availableSlotsRepository.AddSlot(new AvailableSlot
                {
                    Id = e.SlotId.ToString(),
                    DayId = e.DayId,
                    Duration = e.SlotDuration,
                    Date = e.SlotStartTime.Date,
                    StartTime = e.SlotStartTime,
                    IsBooked = false
                }));

            When<SlotBooked>((e, m) =>
                availableSlotsRepository.HideSlot(e.SlotId));

            When<SlotBookingCancelled>((e, m) =>
                availableSlotsRepository.ShowSlot(e.SlotId));

            When<SlotScheduleCancelled>((e, m) =>
                availableSlotsRepository.DeleteSlot(e.SlotId));
        }
    }
}
