using System;
using System.Collections.Generic;
using System.Linq;
using Scheduling.Domain.DoctorDay.Commands;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.DoctorDay.Exceptions;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay
{
    public class Day : AggregateRootSnapshot
    {
        private readonly Slots _slots = new Slots();

        private bool _isArchived;

        private bool _isCancelled;

        private bool _isScheduled;

        public Day()
        {
            Register<DayScheduled>(When);
            Register<SlotScheduled>(When);
            Register<SlotBooked>(When);
            Register<SlotBookingCancelled>(When);
            Register<SlotScheduleCancelled>(When);
            Register<DayScheduleArchived>(When);
            RegisterSnapshot<DaySnapshot>(LoadDaySnapshot, GetDaySnapshot);
        }

        public void Schedule(DoctorId doctorId, DateTime date, List<ScheduledSlot> slots, Func<Guid> idGenerator)
        {
            IsCancelledOrArchived();

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
            IsCancelledOrArchived();
            IsNotScheduled();

            if (_slots.Overlaps(startTime, duration))
                throw new SlotOverlappedException();

            Raise(new SlotScheduled(slotId, Id, startTime, duration));
        }

        public void BookSlot(SlotId slotId, PatientId patientId)
        {
            IsCancelledOrArchived();
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
            IsCancelledOrArchived();
            IsNotScheduled();

            if (!_slots.HasBookedSlot(slotId))
                throw new SlotNotBookedException();

            Raise(new SlotBookingCancelled(Id, slotId, reason));
        }

        public void Archive()
        {
            IsNotScheduled();

            if (_isArchived)
                throw new DayScheduleAlreadyArchivedException();

            Raise(new DayScheduleArchived(Id));
        }

        private void When(DayScheduled @event)
        {
            Id = new DayId(new DoctorId(@event.DoctorId), @event.Date).ToString();
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

        private void When(DayScheduleArchived @event) =>
            _isArchived = true;

        private void IsCancelledOrArchived()
        {
            if (_isArchived)
                throw new DayScheduleAlreadyArchivedException();

            if (_isCancelled)
                throw new DayScheduleAlreadyCancelledException();
        }

        private void IsNotScheduled()
        {
            if (!_isScheduled)
                throw new DayNotScheduledException();
        }

        private object GetDaySnapshot() =>
            new DaySnapshot
            {
                IsArchived = _isArchived,
                IsCancelled = _isCancelled,
                Slots = _slots
                    .All()
                    .Select(s => new SlotSnapshot
                    {
                        Booked = s.Booked,
                        Duration = s.Duration,
                        Id = s.Id,
                        StartTime = s.StartTime
                    }).ToList(),
                IsScheduled = _isScheduled
            };

        private void LoadDaySnapshot(DaySnapshot daySnapshot)
        {
            daySnapshot.Slots.ForEach(s => _slots.Add(s.Id, s.StartTime, s.Duration, s.Booked));
            _isArchived = daySnapshot.IsArchived;
            _isCancelled = daySnapshot.IsCancelled;
            _isScheduled = daySnapshot.IsScheduled;
        }
    }
}
