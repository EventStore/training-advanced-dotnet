using System;

namespace Scheduling.Controllers
{
    public record AvailableSlotsResponse(
        string DayId,
        string SlotId,
        string Date,
        string Time,
        TimeSpan Duration
    );
}
