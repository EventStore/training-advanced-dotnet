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
                
        });

        When<SlotBooked>(async (e, m) =>
        {

        });

        When<SlotBookingCancelled>(async (e, m) =>
        {
                
        });
    }
}