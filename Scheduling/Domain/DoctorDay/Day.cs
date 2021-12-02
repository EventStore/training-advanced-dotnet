using System;
using System.Collections.Generic;
using System.Linq;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.DoctorDay.Exceptions;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay;

public class Day : AggregateRootSnapshot
{
    private readonly Slots _slots = new Slots();

    private bool _isCancelled;

    private bool _isScheduled;

    public Day()
    {
        Register<DayScheduled>(When);
        Register<SlotScheduled>(When);
        Register<SlotBooked>(When);
        Register<SlotBookingCancelled>(When);
        Register<SlotScheduleCancelled>(When);
        Register<DayScheduleCancelled>(When);
        RegisterSnapshot<DaySnapshot>(LoadDaySnapshot, GetDaySnapshot);
    }

    public void Schedule(DoctorId doctorId, DateTime date, List<ScheduledSlot> slots, Func<Guid> idGenerator)
    {
        IsCancelled();

        if (_isScheduled)
            throw new DayAlreadyScheduledException();

        var dayId = new DayId(doctorId, date);
        Raise(new DayScheduled(dayId.Value, doctorId.Value, date));

        foreach (var slot in slots)
        {
            Raise(new SlotScheduled(idGenerator(), dayId.Value, slot.StartTime, slot.Duration));
        }
    }

    public void ScheduleSlot(Guid slotId, DateTime startTime, TimeSpan duration)
    {
        IsCancelled();
        IsNotScheduled();

        if (_slots.Overlaps(startTime, duration))
            throw new SlotOverlappedException();

        Raise(new SlotScheduled(slotId, Id, startTime, duration));
    }

    public void BookSlot(SlotId slotId, PatientId patientId)
    {
        IsCancelled();
        IsNotScheduled();

        var slotStatus = _slots.GetStatus(slotId);

        switch (slotStatus)
        {
            case SlotStatus.Available:
                Raise(new SlotBooked(Id, slotId.Value, patientId.Value));
                break;
            case SlotStatus.Booked:
                throw new SlotAlreadyBookedException();
            case SlotStatus.NotScheduled:
                throw new SlotNotScheduledException();
        }
    }

    public void CancelSlotBooking(Guid slotId, string reason)
    {
        IsCancelled();
        IsNotScheduled();

        if (!_slots.HasBookedSlot(slotId))
            throw new SlotNotBookedException();

        Raise(new SlotBookingCancelled(Id, slotId, reason));
    }

    public void Cancel()
    {
        IsCancelled();
        IsNotScheduled();

        foreach (var bookedSlot in _slots.GetBookedSlots())
        {
            Raise(new SlotBookingCancelled(Id, bookedSlot.Id, null));
        }

        var events = _slots
            .All()
            .Select(slot => new SlotScheduleCancelled(Id, slot.Id))
            .ToList();

        events.ForEach(Raise);

        Raise(new DayScheduleCancelled(Id));
    }

    private void When(DayScheduleCancelled obj)
    {
        _isCancelled = true;
    }

    private void When(DayScheduled @event)
    {
        Id = new DayId(new DoctorId(@event.DoctorId), @event.Date).Value;
        _isScheduled = true;
    }

    private void When(SlotScheduled @event) =>
        _slots.Add(@event.SlotId, @event.SlotStartTime, @event.SlotDuration, false);

    private void When(SlotBooked @event) =>
        _slots.MarkAsBooked(@event.SlotId);

    private void When(SlotBookingCancelled @event) =>
        _slots.MarkAsAvailable(@event.SlotId);

    private void When(SlotScheduleCancelled @event) =>
        _slots.Remove(@event.SlotId);

    private void IsCancelled()
    {
        if (_isCancelled)
            throw new DayScheduleAlreadyCancelledException();
    }

    private void IsNotScheduled()
    {
        if (!_isScheduled)
            throw new DayNotScheduledException();
    }

    private object GetDaySnapshot() =>
        new DaySnapshot(
            _slots
                .All()
                .Select(s => 
                    new SlotSnapshot(
                        s.Id,
                        s.StartTime,
                        s.Duration,
                        s.Booked
                    )
                ).ToList(),
            _isCancelled,
            _isScheduled
        );

    private void LoadDaySnapshot(DaySnapshot daySnapshot)
    {
        daySnapshot.Slots.ForEach(s => _slots.Add(s.Id, s.StartTime, s.Duration, s.Booked));
        _isCancelled = daySnapshot.IsCancelled;
        _isScheduled = daySnapshot.IsScheduled;
    }
}