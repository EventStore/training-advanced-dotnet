using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.Infrastructure.MongoDb;
using Scheduling.Infrastructure.Projections;

namespace Scheduling.Application
{
    public class AvailableSlotsProjectionV2 : EventHandler
    {
        public AvailableSlotsProjectionV2(MongoDbAvailableSlotsRepositoryV2 availableSlotsRepository)
        {
            When<SlotScheduled>(e =>
                availableSlotsRepository.AddSlot(new MongoDbAvailableSlotV2
                {
                    Id = e.SlotId.ToString(),
                    DayId = e.DayId,
                    Duration = e.SlotDuration,
                    Date = e.SlotStartTime.ToString("dd-MM-yyyy"),
                    StartTime = e.SlotStartTime.ToString("h:mm tt"),
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
