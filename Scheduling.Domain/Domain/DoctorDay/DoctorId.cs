using System;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay
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
