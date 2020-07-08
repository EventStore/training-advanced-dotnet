using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
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
