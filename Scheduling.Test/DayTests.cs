using System;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.Domain.DoctorDay;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.DoctorDay.Exceptions;
using Scheduling.Domain.Service;
using Scheduling.Infrastructure.EventStore;
using Scheduling.Test.Test;
using Xunit;

namespace Scheduling.Test;

[Collection("TypeMapper collection")]
public class DayTests : AggregateTest<Day, EventStoreDayRepository>
{
    DoctorId _doctorId;
    DateTime _date;
    DayId _dayId;

    public DayTests()
    {
        RegisterHandlers<Handlers>();

        _doctorId = new DoctorId(Guid.NewGuid());
        _date = new DateTime(2020, 5, 2, 10, 0, 0);
        _dayId = new DayId(_doctorId, _date);
    }

    [Fact]
    public async Task should_be_scheduled()
    {
        var slots = Enumerable.Range(0, 30)
            .Select(r => new ScheduledSlot(TimeSpan.FromMinutes(10), _date.AddMinutes(10 * r)))
            .ToList();

        Given();

        await When(new ScheduleDay(_doctorId.Value, _date, slots));

        Then(events =>
        {
            var dayScheduled = events.First() as DayScheduled;
            Assert.Equal(_doctorId.Value, dayScheduled?.DoctorId);
            Assert.Equal(31, events.Count);
        });
    }

    [Fact]
    public async Task should_not_be_scheduled_twice()
    {
        var slots = Enumerable.Range(0, 30)
            .Select(r => new ScheduledSlot(TimeSpan.FromMinutes(10), _date.AddMinutes(10 * r)))
            .ToList();

        Given(new DayScheduled(_dayId.Value, _doctorId.Value, _date));

        await When(new ScheduleDay(_doctorId.Value, _date, slots));

        Then<DayAlreadyScheduledException>();
    }

    [Fact]
    public async Task should_allow_to_book_slot()
    {
        var slotId = new SlotId(Guid.NewGuid());
        var expected = new SlotBooked(_dayId.Value, slotId.Value, "John Doe");
        Given(
            new DayScheduled(_dayId.Value, _doctorId.Value, _date),
            new SlotScheduled(slotId.Value, _dayId.Value, _date, TimeSpan.FromMinutes(10)));

        await When(new BookSlot(_dayId.Value, slotId.Value, "John Doe"));

        Then(e =>
        {
            Assert.IsType<SlotBooked>(e.First());
            Assert.Equal(expected, e.First());
        });
    }

    [Fact]
    public async Task should_not_allow_to_book_slot_twice()
    {
        var slotId = new SlotId(Guid.NewGuid());

        Given(
            new DayScheduled(_dayId.Value, _doctorId.Value, _date),
            new SlotScheduled(slotId.Value, _dayId.Value, _date, TimeSpan.FromMinutes(10)),
            new SlotBooked(_dayId.Value, slotId.Value, "John Doe"));

        await When(new BookSlot(_dayId.Value, slotId.Value, "John Doe"));

        Then<SlotAlreadyBookedException>();
    }

    [Fact]
    public async Task should_not_allow_to_book_slot_if_day_not_scheduled()
    {
        var slotId = new SlotId(Guid.NewGuid());

        Given();

        await When(new BookSlot(_dayId.Value, slotId.Value, "John Doe"));

        Then<DayNotScheduledException>();
    }

    [Fact]
    public async Task should_not_allow_to_book_an_unscheduled_slot()
    {
        var slotId = new SlotId(Guid.NewGuid());

        Given(new DayScheduled(_dayId.Value, _doctorId.Value, _date));

        await When(new BookSlot(_dayId.Value, slotId.Value, "John Doe"));

        Then<SlotNotScheduledException>();
    }

    [Fact]
    public async Task allow_to_cancel_booking()
    {
        var slotId = new SlotId(Guid.NewGuid());
        var reason = "Cancel Reason";
        var expected = new SlotBookingCancelled(_dayId.Value, slotId.Value, reason);

        Given(
            new DayScheduled(_dayId.Value, _doctorId.Value, _date),
            new SlotScheduled(slotId.Value, _dayId.Value, _date, TimeSpan.FromMinutes(10)),
            new SlotBooked(_dayId.Value, slotId.Value, "John Doe"));

        await When(new CancelSlotBooking(_dayId.Value, slotId.Value, reason));

        Then(e =>
        {
            Assert.IsType<SlotBookingCancelled>(e.First());
            Assert.Equal(expected, e.First());
        });
    }

    [Fact]
    public async Task not_allow_to_cancel_unbooked_slot()
    {
        var slotId = new SlotId(Guid.NewGuid());

        Given(
            new DayScheduled(_dayId.Value, _doctorId.Value, _date),
            new SlotScheduled(slotId.Value, _dayId.Value, _date, TimeSpan.FromMinutes(10)));

        await When(new CancelSlotBooking(_dayId.Value, slotId.Value, "Some Reason"));

        Then<SlotNotBookedException>();
    }

    [Fact]
    public async Task allow_to_schedule_an_extra_slot()
    {
        var slotId = new SlotId(Guid.NewGuid());
        var duration = TimeSpan.FromMinutes(10);
        var expected = new SlotScheduled(slotId.Value, _dayId.Value, _date, duration);
        Given(new DayScheduled(_dayId.Value, _doctorId.Value, _date));

        await When(new ScheduleSlot(slotId.Value, _doctorId.Value, _date, duration, _date));

        Then(e =>
        {
            Assert.IsType<SlotScheduled>(e.First());
            Assert.Equal(expected, e.First());
        });
    }

    [Fact]
    public async Task dont_to_schedule_overlapping_slots()
    {
        var slotOneId = new SlotId(Guid.NewGuid());
        var slotTwoId = new SlotId(Guid.NewGuid());

        Given(
            new DayScheduled(_dayId.Value, _doctorId.Value, _date),
            new SlotScheduled(slotOneId.Value, _dayId.Value, _date, TimeSpan.FromMinutes(10)));

        await When(new ScheduleSlot(slotTwoId.Value, _doctorId.Value, _date, TimeSpan.FromMinutes(10), _date));

        Then<SlotOverlappedException>();
    }

    [Fact]
    public async Task allow_to_schedule_adjacent_slots()
    {
        var slotOneId = new SlotId(Guid.NewGuid());
        var slotTwoId = new SlotId(Guid.NewGuid());
        var duration = TimeSpan.FromMinutes(10);
        var expected = new SlotScheduled(slotTwoId.Value, _dayId.Value, _date.AddMinutes(10), duration);

        Given(
            new DayScheduled(_dayId.Value, _doctorId.Value, _date),
            new SlotScheduled(slotOneId.Value, _dayId.Value, _date, TimeSpan.FromMinutes(10)));

        await When(new ScheduleSlot(slotTwoId.Value, _doctorId.Value, _date, duration,
            _date.AddMinutes(10)));

        Then(e =>
        {
            Assert.IsType<SlotScheduled>(e.First());
            Assert.Equal(expected, e.First());
        });
    }

    [Fact]
    public async Task cancel_booked_slots_when_the_day_is_cancelled()
    {
        var slotOneId = new SlotId(Guid.NewGuid());
        var slotTwoId = new SlotId(Guid.NewGuid());

        var slotCancelledExpected = new SlotBookingCancelled(_dayId.Value, slotOneId.Value, null);
        var slotOneScheduleCancelledExpected = new SlotScheduleCancelled(_dayId.Value, slotOneId.Value);
        var slotTwoScheduleCancelledExpected = new SlotScheduleCancelled(_dayId.Value, slotTwoId.Value);
        var dayCancelledExpected = new DayScheduleCancelled(_dayId.Value);

        Given(
            new DayScheduled(_dayId.Value, _doctorId.Value, _date),
            new SlotScheduled(slotOneId.Value, _dayId.Value, _date, TimeSpan.FromMinutes(10)),
            new SlotScheduled(slotTwoId.Value, _dayId.Value, _date, TimeSpan.FromMinutes(10)),
            new SlotBooked(_dayId.Value, slotOneId.Value, "John Doe"));

        await When(new CancelDaySchedule(_dayId.Value));

        Then(e =>
        {
            Assert.IsType<SlotBookingCancelled>(e[0]);
            Assert.Equal(slotCancelledExpected, e[0]);

            Assert.IsType<SlotScheduleCancelled>(e[1]);
            Assert.Equal(slotOneScheduleCancelledExpected, e[1]);

            Assert.IsType<SlotScheduleCancelled>(e[2]);
            Assert.Equal(slotTwoScheduleCancelledExpected, e[2]);

            Assert.IsType<DayScheduleCancelled>(e[3]);
            Assert.Equal(dayCancelledExpected, e[3]);
        });
    }

    [Fact]
    public async Task archive_scheduled_day()
    {
        var expected = new DayScheduleArchived(_dayId.Value);

        Given(new DayScheduled(_dayId.Value, _doctorId.Value, _date));
        await When(new ArchiveDaySchedule(_dayId.Value));
        Then(e =>
        {
            Assert.IsType<DayScheduleArchived>(e.First());
            Assert.Equal(expected,e.First());
        });
    }

    [Fact]
    public void archive_cancelled_day()
    {
    }
}