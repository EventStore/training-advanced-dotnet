using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EventStore.Client;
using MongoDB.Driver;
using Scheduling.Application;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.Domain.ReadModel;
using Scheduling.Infrastructure.EventStore;
using Scheduling.Infrastructure.MongoDb;
using Scheduling.Test.Test;
using Xunit;
using EventHandler = Scheduling.Infrastructure.Projections.EventHandler;

namespace Scheduling.Test;

[Collection("TypeMapper collection")]
public class OverbookingProcessManagerTest : HandlerTest
{
    private readonly DateTime _now = DateTime.UtcNow;

    private readonly TimeSpan _tenMinutes = TimeSpan.FromMinutes(10);

    private EsEventStore _esStore;

    private EsCommandStore _esCommandStore;

    private IBookedSlotsRepository _repository;

    public OverbookingProcessManagerTest()
    {
        _esStore = new EsEventStore(GetEventStoreClient(), "test");
        _esCommandStore = new EsCommandStore(_esStore, null!, null!, null!);

        var mongoClient = new MongoClient("mongodb://localhost");
        _repository = new MongoDbBookedSlotRepository(mongoClient.GetDatabase(Guid.NewGuid().ToString()));
    }

    protected override EventHandler GetHandler()
    {
        return new OverbookingProcessManager(
            _repository,
            3,
            _esCommandStore,
            Guid.NewGuid
        );
    }

    [Fact]
    public async Task should_increment_the_visit_counter_when_slot_is_booked()
    {
        var dayId = Guid.NewGuid().ToString();
        var slotSchedule1 = new SlotScheduled(Guid.NewGuid(), dayId, _now, _tenMinutes);
        var slotSchedule2 = new SlotScheduled(Guid.NewGuid(), dayId, _now.AddMinutes(10), _tenMinutes);
        var slotBooked1 = new SlotBooked(dayId, slotSchedule1.SlotId, "patient 1");
        var slotBooked2 = new SlotBooked(dayId, slotSchedule2.SlotId, "patient 1");

        await Given(slotSchedule1, slotSchedule2, slotBooked1, slotBooked2);
        Then(2, await _repository.CountByPatientAndMonth("patient 1", _now.Month));
    }

    [Fact]
    public async Task should_decrement_the_visit_counter_when_slot_booking_is_cancelled()
    {
        var dayId = Guid.NewGuid().ToString();
        var slotSchedule1 = new SlotScheduled(Guid.NewGuid(), dayId, _now, _tenMinutes);
        var slotSchedule2 = new SlotScheduled(Guid.NewGuid(), dayId, _now.AddMinutes(10), _tenMinutes);
        var slotBooked1 = new SlotBooked(dayId, slotSchedule1.SlotId, "patient 1");
        var slotBooked2 = new SlotBooked(dayId, slotSchedule2.SlotId, "patient 1");
        var slotBookingCancelled = new SlotBookingCancelled(dayId, slotSchedule2.SlotId, "no longer needed");

        await Given(slotSchedule1, slotSchedule2, slotBooked1, slotBooked2, slotBookingCancelled);
        Then(1, await _repository.CountByPatientAndMonth("patient 1", _now.Month));
    }

    [Fact]
    public async Task should_issue_command_to_cancel_slot_if_booking_limit_was_reached()
    {
        var dayId = Guid.NewGuid().ToString();
        var slotSchedule1 = new SlotScheduled(Guid.NewGuid(), dayId, _now, _tenMinutes);
        var slotSchedule2 = new SlotScheduled(Guid.NewGuid(), dayId, _now.AddMinutes(10), _tenMinutes);
        var slotSchedule3 = new SlotScheduled(Guid.NewGuid(), dayId, _now.AddMinutes(20), _tenMinutes);
        var slotSchedule4 = new SlotScheduled(Guid.NewGuid(), dayId, _now.AddMinutes(30), _tenMinutes);
        var slotBooked1 = new SlotBooked(dayId, slotSchedule1.SlotId, "patient 1");
        var slotBooked2 = new SlotBooked(dayId, slotSchedule2.SlotId, "patient 1");
        var slotBooked3 = new SlotBooked(dayId, slotSchedule3.SlotId, "patient 1");
        var slotBooked4 = new SlotBooked(dayId, slotSchedule4.SlotId, "patient 1");

        await Given(slotSchedule1, slotSchedule2, slotSchedule3, slotSchedule4, slotBooked1, slotBooked2, slotBooked3,
            slotBooked4);

        Then(
            new CancelSlotBooking(dayId, slotBooked4.SlotId, "overbooked"),
            (await _esStore.LoadCommands("async_command_handler-day")).Last().Command);
    }

    private static EventStoreClient GetEventStoreClient() =>
        new(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false"));
}