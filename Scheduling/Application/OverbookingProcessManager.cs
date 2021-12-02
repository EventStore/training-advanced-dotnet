using System;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.Domain.ReadModel;
using Scheduling.Domain.ReadModel;
using Scheduling.EventSourcing;
using EventHandler = Scheduling.Infrastructure.Projections.EventHandler;

namespace Scheduling.Application;

public class OverbookingProcessManager : EventHandler
{
    public OverbookingProcessManager(IBookedSlotsRepository bookedSlotRepository, int bookingLimitedPerPatient,
        ICommandStore commandStore, Func<Guid> idGenerator)
    {
        When<SlotScheduled>(async (e, m) =>
        {
            await bookedSlotRepository.AddSlot(new BookedSlot(e.SlotId.ToString(), e.DayId, e.SlotStartTime.Month));
        });

        When<SlotBooked>(async (e, m) =>
        {
            await bookedSlotRepository.MarkSlotAsBooked(e.SlotId.ToString(), e.PatientId);

            var slot = await bookedSlotRepository.GetSlot(e.SlotId.ToString());
            var count = await bookedSlotRepository.CountByPatientAndMonth(e.PatientId, slot.Month);

            if (count > bookingLimitedPerPatient)
            {
                await commandStore.Send(new CancelSlotBooking(e.DayId, e.SlotId, "overbooked"),
                    new CommandMetadata(m.CorrelationId, new CausationId(idGenerator())));
            }
        });

        When<SlotBookingCancelled>(async (e, m) =>
        {
            await bookedSlotRepository.MarkSlotAsAvailable(e.SlotId.ToString());
        });
    }
}