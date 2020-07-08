using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay
{
    public class PatientId : Value<PatientId>
    {
        public string Value { get; }

        public PatientId(string value)
        {
            Value = value;
        }
    }
}
