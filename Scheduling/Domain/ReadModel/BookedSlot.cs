namespace Scheduling.Domain.ReadModel
{
    public record BookedSlot(
        string Id,
        string DayId,
        int Month,
        string? PatientId = null,
        bool IsBooked = false
    );
}
