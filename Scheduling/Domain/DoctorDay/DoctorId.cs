using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay
{
    public class DoctorId : Value<DoctorId>
    {
        public Guid Value { get; }

        public DoctorId(Guid value)
        {
            Value = value;
        }
    }
}
