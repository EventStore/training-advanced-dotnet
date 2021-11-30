using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling.Domain.DoctorDay
{
    public class Slots
    {
        private readonly List<Slot> _slots = new();

        public void Add(Guid slotId, DateTime slotStartTime, TimeSpan slotDuration, bool booked) =>
            _slots.Add(new Slot(slotId, slotStartTime, slotDuration, booked));

        public bool Overlaps(DateTime slotStartTime, TimeSpan slotDuration) =>
            _slots.Any(slot => slot.OverlapsWith(slotStartTime, slotDuration));

        public IEnumerable<Slot> All() =>
            _slots;

        public SlotStatus GetStatus(SlotId slotId)
        {
            var slot = _slots.FirstOrDefault(s => s.Id == slotId.Value);

            if (slot == null)
                return SlotStatus.NotScheduled;

            return slot.Booked ? SlotStatus.Booked : SlotStatus.Available;
        }

        public void MarkAsBooked(Guid slotId)
        {
            var slot = _slots.FirstOrDefault(s => s.Id == slotId);
            slot?.Book();
        }

        public bool HasBookedSlot(Guid slotId)
        {
            var slot = _slots.FirstOrDefault(s => s.Id == slotId);

            if (slot != null)
                return slot.Booked;

            return false;
        }

        public void MarkAsAvailable(Guid slotId)
        {
            var slot = _slots.FirstOrDefault(s => s.Id == slotId);
            slot?.Cancel();
        }

        public IEnumerable<Slot> GetBookedSlots() =>
            _slots.Where(s => s.Booked);

        public void Remove(Guid slotId) =>
            _slots.RemoveAll(s => s.Id == slotId);
    }
}
