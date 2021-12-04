using System;
using Newtonsoft.Json.Linq;
using static Scheduling.EventSourcing.TypeMapper;

namespace Scheduling.Domain.DoctorDay.Events;

public static class EventMappings
{
    private const string Prefix = "doctorday";

    public static void MapEventTypes()
    {
        Map<DayScheduled>($"{Prefix}-day-scheduled",
            d => new DayScheduled(
                d["dayId"]!.ToString(),
                Guid.Parse(d["doctorId"]!.ToString()),
                Convert.ToDateTime(d["date"])
            ),
            o => new JObject
            {
                ["dayId"] = o.DayId,
                ["doctorId"] = o.DoctorId,
                ["date"] = o.Date
            }
        );

        Map<SlotScheduled>($"{Prefix}-slot-scheduled",
            d => new SlotScheduled(
                Guid.Parse(d["slotId"]!.ToString()),
                d["dayId"]!.ToString(),
                Convert.ToDateTime(d["slotStartTime"]),
                TimeSpan.Parse(d["slotDuration"]!.ToString())
            ),
            o => new JObject
            {
                ["slotId"] = o.SlotId,
                ["dayId"] = o.DayId,
                ["slotStartTime"] = o.SlotStartTime,
                ["slotDuration"] = o.SlotDuration
            });

        Map<SlotBooked>($"{Prefix}-slot-booked",
            d => new SlotBooked(
                d["dayId"]!.ToString(),
                Guid.Parse(d["slotId"]!.ToString()),
                d["patientId"]!.ToString()
            ),
            o => new JObject
            {
                ["dayId"] = o.DayId,
                ["slotId"] = o.SlotId,
                ["patientId"] = o.PatientId
            });

        Map<SlotBookingCancelled>($"{Prefix}-slot-booking-cancelled",
            d => new SlotBookingCancelled(
                d["dayId"]!.ToString(),
                Guid.Parse(d["slotId"]!.ToString()),
                d["reason"]!.ToString()
            ), o => new JObject
            {
                ["dayId"] = o.DayId,
                ["slotId"] = o.SlotId,
                ["reason"] = o.Reason
            });

        Map<SlotScheduleCancelled>($"{Prefix}-slot-schedule-cancelled",
            d => new SlotScheduleCancelled(
                d["dayId"]!.ToString(),
                Guid.Parse(d["slotId"]!.ToString())),
            o => new JObject
            {
                ["dayId"] = o.DayId,
                ["slotId"] = o.SlotId
            });

        Map<DayScheduleCancelled>($"{Prefix}-day-schedule-cancelled",
            d => new DayScheduleCancelled(
                d["dayId"]!.ToString()),
            o => new JObject
            {
                ["dayId"] = o.DayId
            });

        Map<DayScheduleArchived>($"{Prefix}-day-schedule-archived",
            d => new DayScheduleArchived(d["dayId"]!.ToString()),
            o => new JObject
            {
                ["dayId"] = o.DayId
            });

        Map<CalendarDayStarted>($"{Prefix}-calendar-day-started",
            d => new CalendarDayStarted(DateTime.Parse(d["date"]!.ToString())),
            o => new JObject
            {
                ["date"] = o.Date
            });

        Map<DaySnapshot>("doctor-day-snapshot",
            d => d.ToObject<DaySnapshot>()!,
            JObject.FromObject
        );
    }
}