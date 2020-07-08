using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scheduling.Domain.ReadModel
{
    public interface IAvailableSlotsRepository
    {
        Task<List<AvailableSlot>> GetAvailableSlotsOn(DateTime today);

        Task AddSlot(AvailableSlot availableSlot);

        Task HideSlot(Guid slotId);

        Task ShowSlot(Guid slotId);

        Task DeleteSlot(Guid slotId);
    }
}
