using System;
using Scheduling.Infrastructure.MongoDb;

namespace Scheduling.Domain.ReadModel
{
    public class AvailableSlot : Document
    {
        public string DayId { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsBooked { get; set; }

        protected bool Equals(AvailableSlot other)
        {
            return DayId == other.DayId && Date.Equals(other.Date) && StartTime.ToString().Equals(other.StartTime.ToString()) && Duration.Equals(other.Duration) && IsBooked == other.IsBooked;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AvailableSlot) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DayId, Date, StartTime, Duration, IsBooked);
        }
    }
}
