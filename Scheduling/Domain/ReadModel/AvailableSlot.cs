using System;

namespace Scheduling.Domain.ReadModel
{
    public record AvailableSlot(
        string Id,
        string DayId,
        string Date,
        string StartTime,
        TimeSpan Duration
    );
}
