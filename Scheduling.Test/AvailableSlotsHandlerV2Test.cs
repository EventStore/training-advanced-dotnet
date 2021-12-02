using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Scheduling.Application;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.Infrastructure.MongoDb;
using Scheduling.Test.Test;
using Xunit;
using EventHandler = Scheduling.Infrastructure.Projections.EventHandler;

namespace Scheduling.Test
{
    [Collection("TypeMapper collection")]
    public class AvailableSlotsHandlerV2Test : HandlerTest
    {
        private static MongoDbAvailableSlotsRepositoryV2 _repository = default!;

        private readonly DateTime _now = DateTime.UtcNow;

        private readonly TimeSpan _tenMinutes = TimeSpan.FromMinutes(10);

        public AvailableSlotsHandlerV2Test()
        {
            // Repeats every event 2x, e.g.: 1 1 2 2 3 3            
            EnableAtLeastOnceMonkey = false;
            // Repeats all elements except last e.g.: 1 2 3 1 2            
            EnableAtLeastOnceGorilla = false;
        }

        protected override EventHandler GetHandler()
        {
            var mongoClient = new MongoClient("mongodb://localhost");
            _repository = new MongoDbAvailableSlotsRepositoryV2(mongoClient.GetDatabase(Guid.NewGuid().ToString()));
            return new AvailableSlotsProjectionV2(_repository);
        }

        [Fact]
        public async Task should_add_slot_to_the_list()
        {
            var scheduled = new SlotScheduled(Guid.NewGuid(), "dayId", _now, _tenMinutes);
            await Given(scheduled);
            Then(
                new AvailableSlot(
                    scheduled.SlotId.ToString(),
                    scheduled.DayId,
                    scheduled.SlotStartTime.Date.ToString("dd-MM-yyyy"),
                    scheduled.SlotStartTime.ToString("h:mm tt"),
                    scheduled.SlotDuration
                ), (await _repository.GetAvailableSlotsOn(_now)).First());
        }

        [Fact]
        public async Task should_hide_the_slot_from_list_if_booked()
        {
            var scheduled = new SlotScheduled(Guid.NewGuid(), "dayId", _now, _tenMinutes);
            await Given(
                scheduled,
                new SlotBooked("dayId", scheduled.SlotId, "PatientId"));
            Then(new List<AvailableSlot>(), await _repository.GetAvailableSlotsOn(_now));
        }

        [Fact]
        public async Task should_show_slot_if_booking_was_cancelled()
        {
            var scheduled = new SlotScheduled(Guid.NewGuid(), "dayId", _now, _tenMinutes);
            await Given(
                scheduled,
                new SlotBooked("dayId", scheduled.SlotId, "PatientId"),
                new SlotBookingCancelled("dayId", scheduled.SlotId, "Reason"));
            Then(new List<AvailableSlot>
            {
                new AvailableSlot(
                    scheduled.SlotId.ToString(),
                    scheduled.DayId,
                    scheduled.SlotStartTime.Date.ToString("dd-MM-yyyy"),
                    scheduled.SlotStartTime.ToString("h:mm tt"),
                    scheduled.SlotDuration
                )
            }, await _repository.GetAvailableSlotsOn(_now));
        }

        [Fact]
        public async Task should_delete_slot_if_slot_was_cancelled()
        {
            var scheduled = new SlotScheduled(Guid.NewGuid(), "dayId", _now, _tenMinutes);
            await Given(
                scheduled,
                new SlotScheduleCancelled("dayId", scheduled.SlotId));
            Then(new List<AvailableSlot>(), await _repository.GetAvailableSlotsOn(_now));
        }
    }
}
