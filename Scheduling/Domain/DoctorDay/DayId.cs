using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay
{
    public class DayId : Value<DayId>
    {
        public string Value { get; }

        public DayId(string value)
        {
            Value = value;
        }

        public DayId(DoctorId doctorId, DateTime date)
        {
            Value = $"{doctorId.Value}_{date.Date:yyyy-MM-dd}";
        }
    }
}
