using System;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb;

public record MongoDbAvailableSlotV2(
    string Id,
    string DayId,
    string Date,
    string StartTime,
    TimeSpan Duration,
    bool IsBooked
)
{
    public AvailableSlot ToAvailableSlot()
    {
        return new AvailableSlot(Id, DayId, Date, StartTime, Duration);
    }
}