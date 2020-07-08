using System;
using Scheduling.Domain.Infrastructure.MongoDb;

namespace Scheduling.Domain.Domain.ReadModel
{
    public class AvailableSlot : Document<AvailableSlot>
    {
        public string DayId { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsBooked { get; set; }
    }
}
