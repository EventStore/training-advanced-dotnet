using System;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.Infrastructure.MongoDb;
using EventHandler = Scheduling.Infrastructure.Projections.EventHandler;

namespace Scheduling.Application
{
    public class AvailableSlotsProjectionV2 : EventHandler
    {
        public AvailableSlotsProjectionV2(MongoDbAvailableSlotsRepositoryV2 availableSlotsRepository)
        {
            When<SlotScheduled>(e => { throw new NotImplementedException(); });

            When<SlotBooked>(e => { throw new NotImplementedException(); });

            // Add when for SlotBookingCancelled
        }
    }
}
