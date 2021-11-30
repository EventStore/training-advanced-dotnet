using System;

namespace Scheduling.Domain.ReadModel
{
    public record AvailableSlot(
        string Id,
        string DayId,
        DateTime Date,
        DateTime StartTime,
        TimeSpan Duration,
        bool IsBooked
    );
}
