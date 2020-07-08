using System;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Events
{
    public class DayScheduled : Event<DayScheduled>
    {
        public string DayId { get; }

        public Guid DoctorId { get; }

        public DateTime Date { get; }

        public DayScheduled(string dayId, Guid doctorId, in DateTime date)
        {
            DayId = dayId;
            DoctorId = doctorId;
            Date = date;
        }
    }
}
