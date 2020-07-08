using System;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Events
{
    public class CalendarDayStarted : Event<CalendarDayStarted>
    {
        public DateTime Date { get; }

        public CalendarDayStarted(DateTime date)
        {
            Date = date;
        }
    }
}
