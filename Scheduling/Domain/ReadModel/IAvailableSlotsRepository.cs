using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scheduling.Domain.ReadModel
{
    public interface IAvailableSlotsRepository
    {
        Task<List<AvailableSlot>> GetAvailableSlotsOn(DateTime today);
    }
}
