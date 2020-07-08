using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay
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
