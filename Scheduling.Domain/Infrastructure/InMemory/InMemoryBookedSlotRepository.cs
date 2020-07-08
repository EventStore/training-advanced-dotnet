using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Scheduling.Domain.Domain.ReadModel;
using Scheduling.Domain.Infrastructure.MongoDb;

namespace Scheduling.Domain.Infrastructure.InMemory
{
    public class InMemoryBookedSlotRepository : IBookedSlotsRepository
    {
        private static readonly List<BookedSlot> _available = new List<BookedSlot>();

        private static readonly List<BookedSlot> _booked = new List<BookedSlot>();

        public Task<int> CountByPatientAndMonth(string patientId, int month)
        {
            throw new System.NotImplementedException();
        }

        public Task AddSlot(BookedSlot slot)
        {
            _available.Add(slot);
            return Task.CompletedTask;
        }

        public Task MarkSlotAsBooked(string slotId, string patientId)
        {
            var slot = _available.First(s => s.Id == slotId.ToString());
            _available.Remove(slot);
            _booked.Add(slot);
            return Task.CompletedTask;
        }

        public Task MarkSlotAsAvailable(string slotId)
        {
            throw new System.NotImplementedException();
        }

        public Task<BookedSlot> GetSlot(string slotId)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SlotDataRow : Document<SlotDataRow>
    {
        public string Id { get; set; }

        private String slotId;
        private String dayId;
        private int monthNumber;
    }
}
