using System;
using System.Collections.Generic;

namespace Scheduling.Domain.DoctorDay;

public record DaySnapshot(
    List<SlotSnapshot> Slots,
    bool IsCancelled,
    bool IsScheduled,
    bool IsArchived = false
);

public record SlotSnapshot(
    Guid Id,
    DateTime StartTime,
    TimeSpan Duration,
    bool Booked
);