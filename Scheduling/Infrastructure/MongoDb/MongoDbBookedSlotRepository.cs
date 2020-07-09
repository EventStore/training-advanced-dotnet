using System.Threading.Tasks;
using MongoDB.Driver;
using Scheduling.Domain.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb
{
    public class MongoDbBookedSlotRepository : IBookedSlotsRepository
    {
        private IMongoDatabase _database;
        private IMongoCollection<BookedSlot> _collection;

        public MongoDbBookedSlotRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<BookedSlot>("booked_slot");
        }

        public async Task<int> CountByPatientAndMonth(string patientId, int month)
        {
            var result = await _collection.FindAsync(x => x.PatientId == patientId && x.Month == month);
            return result.ToList().Count;
        }

        public Task AddSlot(BookedSlot slot)
        {
            return _collection.InsertOneAsync(slot);
        }

        public Task MarkSlotAsBooked(string slotId, string patientId)
        {
            var filter = Builders<BookedSlot>.Filter.Eq(x => x.Id, slotId);
            var update = Builders<BookedSlot>.Update
                .Set(a => a.IsBooked, true)
                .Set(a => a.PatientId, patientId);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task MarkSlotAsAvailable(string slotId)
        {
            var filter = Builders<BookedSlot>.Filter.Eq(x => x.Id, slotId);
            var update = Builders<BookedSlot>.Update
                .Set(a => a.IsBooked, false)
                .Set(a => a.PatientId, string.Empty);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task<BookedSlot> GetSlot(string slotId)
        {
            return (await _collection.FindAsync(x => x.Id == slotId)).FirstOrDefault();

        }
    }
}
