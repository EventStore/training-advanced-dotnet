using System.Threading.Tasks;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Domain.Domain.ReadModel;

public interface IBookedSlotsRepository
{
    Task<int> CountByPatientAndMonth(string patientId, int month);

    Task AddSlot(BookedSlot slot);

    Task MarkSlotAsBooked(string slotId, string patientId);

    Task MarkSlotAsAvailable(string slotId);

    Task<BookedSlot> GetSlot(string slotId);
}