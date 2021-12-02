using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay;

public record SlotId(
    Guid Value
);