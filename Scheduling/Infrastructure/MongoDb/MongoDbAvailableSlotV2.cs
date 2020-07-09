using System;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb
{
    public class MongoDbAvailableSlotV2 : AvailableSlot
    {
        public bool IsBooked { get; set; }
        
        protected bool Equals(MongoDbAvailableSlot other)
        {
            return DayId == other.DayId && Date.Equals(other.Date) && StartTime.ToString().Equals(other.StartTime.ToString()) && Duration.Equals(other.Duration) && IsBooked == other.IsBooked;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MongoDbAvailableSlot) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DayId, Date, StartTime, Duration, IsBooked);
        }
    }
}
