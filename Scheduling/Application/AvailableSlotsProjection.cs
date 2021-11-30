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
                availableSlotsRepository.AddSlot(new MongoDbAvailableSlot(
                    e.SlotId.ToString(),
                    e.DayId,
                    e.SlotStartTime.ToString("dd-MM-yyyy"),
                    e.SlotStartTime.ToString("h:mm tt"),
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
