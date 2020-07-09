using System;
using Scheduling.Infrastructure.MongoDb;

namespace Scheduling.Domain.ReadModel
{
    public class AvailableSlot
    {
        public string DayId { get; set; }

        public string Date { get; set; }

        public string StartTime { get; set; }

        public TimeSpan Duration { get; set; }
        
        public string Id { get; set; }

        protected bool Equals(AvailableSlot other)
        {
            return DayId == other.DayId && Date == other.Date && StartTime == other.StartTime && Duration.Equals(other.Duration) && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is AvailableSlot)) return false;
            return Equals((AvailableSlot) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DayId, Date, StartTime, Duration, Id);
        }
    }
}
