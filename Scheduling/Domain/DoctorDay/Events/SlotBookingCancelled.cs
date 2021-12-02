using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events;

public record SlotBookingCancelled(
    string DayId,
    Guid SlotId,
    string? Reason
) : IEvent;