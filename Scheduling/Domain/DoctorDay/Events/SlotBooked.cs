using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events;

public record SlotBooked(
    string DayId,
    Guid SlotId,
    string PatientId
) : IEvent;