using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.Domain.Domain.ReadModel;

namespace Application.Infrastructure.InMemory
{
    public class InMemoryAvailableSlotsRepository: IAvailableSlotsRepository
    {
        private static readonly List<AvailableSlot> _available = new List<AvailableSlot>();
        private static readonly List<AvailableSlot> _booked = new List<AvailableSlot>();

        public void Clear()
        {
            _available.Clear();
            _booked.Clear();
        }

        public Task<List<AvailableSlot>> GetAvailableSlotsOn(DateTime today)
        {
            return Task.FromResult(_available);
        }

        public Task AddSlot(AvailableSlot availableSlot)
        {
            _available.Add(availableSlot);
            return Task.CompletedTask;
        }

        public Task HideSlot(Guid slotId)
        {
            var slot = _available.First(s => s.Id == slotId.ToString());
            _available.Remove(slot);
            _booked.Add(slot);
            return Task.CompletedTask;
        }

        public Task ShowSlot(Guid slotId)
        {
            var slot = _booked.First(s => s.Id == slotId.ToString());
            _booked.Remove(slot);
            _available.Add(slot);
            return Task.CompletedTask;
        }

        public Task DeleteSlot(Guid slotId)
        {
            _booked.RemoveAll(b => b.Id == slotId.ToString());
            _available.RemoveAll(b => b.Id == slotId.ToString());
            return Task.CompletedTask;
        }
    }
}
