using System;

namespace Scheduling.Controllers
{
    public class AvailableSlotsResponse
    {
        public string DayId { get; set; }

        public string SlotId { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public TimeSpan Duration  { get; set; }
    }
}
