using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events;

public record DayScheduled(
    string DayId,
    Guid DoctorId,
    DateTime Date
) : IEvent;