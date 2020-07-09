using System;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.Domain.ReadModel;
using Scheduling.EventSourcing;
using EventHandler = Scheduling.Infrastructure.Projections.EventHandler;


namespace Scheduling.Domain.Application
{
    public class OverbookingProcessManager : EventHandler
    {
        public OverbookingProcessManager(IBookedSlotsRepository bookedSlotRepository, int bookingLimitedPerPatient,
            ICommandStore commandStore, Func<Guid> idGenerator)
        {
            When<SlotScheduled>(async e =>
            {
                
            });

            When<SlotBooked>(async e =>
            {

            });

            When<SlotBookingCancelled>(async e =>
            {
                
            });
        }
    }
}
