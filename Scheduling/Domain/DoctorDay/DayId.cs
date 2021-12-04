using System;

namespace Scheduling.Domain.DoctorDay;

public record DayId(
    string Value
)
{
    public DayId(DoctorId doctorId, DateTime date) 
        : this($"{doctorId.Value}_{date.Date:yyyy-MM-dd}")
    {
    }
}