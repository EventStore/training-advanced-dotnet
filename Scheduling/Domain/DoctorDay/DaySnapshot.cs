using System;
using System.Collections.Generic;

namespace Scheduling.Domain.DoctorDay
{
    public record DaySnapshot(
        List<SlotSnapshot> Slots,
        bool IsArchived,
        bool IsCancelled,
        bool IsScheduled
    );

    public record SlotSnapshot(
        Guid Id,
        DateTime StartTime,
        TimeSpan Duration,
        bool Booked
    );
}
