using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.Infrastructure.MongoDb;
using Scheduling.Infrastructure.Projections;

namespace Scheduling.Application
{
    public class AvailableSlotsProjection : EventHandler
    {
        public AvailableSlotsProjection(MongoDbAvailableSlotsRepository availableSlotsRepository)
        {
            When<SlotScheduled>((e, m) =>
                availableSlotsRepository.AddSlot(new MongoDbAvailableSlot
                {
                    Id = e.SlotId.ToString(),
                    DayId = e.DayId,
                    Duration = e.SlotDuration,
                    Date = e.SlotStartTime.ToString("dd-MM-yyyy"),
                    StartTime = e.SlotStartTime.ToString("h:mm tt"),
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
