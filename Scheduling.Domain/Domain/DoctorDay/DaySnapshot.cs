using System;
using System.Collections.Generic;

namespace Scheduling.Domain.Domain.DoctorDay
{
    public class DaySnapshot
    {
        public List<SlotSnapshot> Slots { get; set; }

        public bool IsArchived { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsScheduled { get; set; }
    }

    public class SlotSnapshot
    {
        public Guid Id { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public bool Booked { get; set; }
    }
}
