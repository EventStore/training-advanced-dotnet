using System;
using Newtonsoft.Json.Linq;
using static Scheduling.Domain.EventSourcing.TypeMapper;

namespace Scheduling.Domain.Domain.DoctorDay.Events
{
    public class EventMappings
    {
        private static string prefix = "doctorday";

        public static void MapEventTypes()
        {
            Map<DayScheduled>($"{prefix}-day-scheduled",
                d => new DayScheduled(
                    d["dayId"].ToString(),
                    Guid.Parse(d["doctorId"].ToString()),
                    Convert.ToDateTime(d["date"])
                ),
                o => new JObject
                {
                    ["dayId"] = o.DayId,
                    ["doctorId"] = o.DoctorId,
                    ["date"] = o.Date
                }
            );

            Map<SlotScheduled>($"{prefix}-slot-scheduled",
                d => new SlotScheduled(
                    Guid.Parse(d["slotId"].ToString()),
                    d["dayId"].ToString(),
                    Convert.ToDateTime(d["slotStartTime"]),
                    TimeSpan.Parse(d["slotDuration"].ToString())
                ),
                o => new JObject
                {
                    ["slotId"] = o.SlotId,
                    ["dayId"] = o.DayId,
                    ["slotStartTime"] = o.SlotStartTime,
                    ["slotDuration"] = o.SlotDuration
                });

            Map<SlotBooked>($"{prefix}-slot-booked",
                d => new SlotBooked(
                    d["dayId"].ToString(),
                    Guid.Parse(d["slotId"].ToString()),
                    d["patientId"].ToString()
                ),
                o => new JObject
                {
                    ["dayId"] = o.DayId,
                    ["slotId"] = o.SlotId,
                    ["patientId"] = o.PatientId
                });

            Map<SlotBookingCancelled>($"{prefix}-slot-booking-cancelled",
                d => new SlotBookingCancelled(
                    d["dayId"].ToString(),
                    Guid.Parse(d["slotId"].ToString()),
                    d["reason"].ToString()
                ), o => new JObject
                {
                    ["dayId"] = o.DayId,
                    ["slotId"] = o.SlotId,
                    ["patientId"] = o.Reason
                });

            Map<SlotScheduleCancelled>($"{prefix}-slot-schedule-cancelled",
                d => new SlotScheduleCancelled(
                    d["dayId"].ToString(),
                    Guid.Parse(d["slotId"].ToString())),
                o => new JObject
                {
                    ["dayId"] = o.DayId,
                    ["slotId"] = o.SlotId
                });

            Map<DayScheduleCancelled>($"{prefix}-day-schedule-cancelled",
                d => new DayScheduleCancelled(
                    d["dayId"].ToString()),
                o => new JObject
                {
                    ["dayId"] = o.DayId
                });

            Map<DayScheduleArchived>($"{prefix}-day-schedule-archived",
                d => new DayScheduleArchived(d["dayId"].ToString()),
                o => new JObject
                {
                    ["dayId"] = o.DayId
                });

            Map<CalendarDayStarted>($"{prefix}-calendar-day-started",
                d => new CalendarDayStarted(DateTime.Parse(d["date"].ToString())),
                o => new JObject
                {
                    ["date"] = o.Date
                });

            Map<DaySnapshot>("doctor-day-snapshot",
                d => d.ToObject<DaySnapshot>(),
                JObject.FromObject
            );
        }
    }
}
