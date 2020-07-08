using System;
using Scheduling.Domain.Domain.DoctorDay.Commands;
using Scheduling.Domain.Domain.DoctorDay.Events;
using Scheduling.Domain.Domain.ReadModel;
using Scheduling.Domain.EventSourcing;
using EventHandler = Scheduling.Domain.Infrastructure.Projections.EventHandler;

namespace Scheduling.Domain.Application
{
    public class OverbookingProcessManager : EventHandler
    {
        public OverbookingProcessManager(IBookedSlotsRepository bookedSlotRepository, int bookingLimitedPerPatient,
            ICommandStore commandStore, Func<Guid> idGenerator)
        {
            // Isn't needed
            When<SlotScheduled>(async e =>
            {
                await bookedSlotRepository.AddSlot(new BookedSlot
                {
                    Id = e.SlotId.ToString(),
                    Month = e.SlotStartTime.Month,
                    DayId = e.DayId
                });
            });

            When<SlotBooked>(async e =>
            {
                await bookedSlotRepository.MarkSlotAsBooked(e.SlotId.ToString(), e.PatientId);

                var slot = await bookedSlotRepository.GetSlot(e.SlotId.ToString());
                var count = await bookedSlotRepository.CountByPatientAndMonth(e.PatientId, slot.Month);

                if (count > bookingLimitedPerPatient)
                {
                    await commandStore.Send(new CancelSlotBooking(e.DayId, e.SlotId, "overbooked"),
                        new CommandMetadata(new CorrelationId(idGenerator()), new CausationId(idGenerator())));
                }
            });

            When<SlotBookingCancelled>(async e =>
            {
                await bookedSlotRepository.MarkSlotAsAvailable(e.SlotId.ToString());
            });
        }
    }
}
