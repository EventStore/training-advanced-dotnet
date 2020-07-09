using System.Threading.Tasks;
using MongoDB.Driver;
using Scheduling.Domain.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb
{
    public class MongoDbBookedSlotRepository : IBookedSlotsRepository
    {
        private IMongoDatabase _database;

        public MongoDbBookedSlotRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<int> CountByPatientAndMonth(string patientId, int month)
        {
            var result = await _database.For<BookedSlot>().FindAsync(x => x.PatientId == patientId && x.Month == month);
            return result.ToList().Count;
        }

        public Task AddSlot(BookedSlot slot)
        {
            return _database.For<BookedSlot>().InsertOneAsync(slot);
        }

        public Task MarkSlotAsBooked(string slotId, string patientId)
        {
            var filter = Builders<BookedSlot>.Filter.Eq(x => x.Id, slotId);
            var update = Builders<BookedSlot>.Update
                .Set(a => a.IsBooked, true)
                .Set(a => a.PatientId, patientId);
            return _database.For<BookedSlot>().UpdateOneAsync(filter, update);
        }

        public Task MarkSlotAsAvailable(string slotId)
        {
            var filter = Builders<BookedSlot>.Filter.Eq(x => x.Id, slotId);
            var update = Builders<BookedSlot>.Update
                .Set(a => a.IsBooked, false)
                .Set(a => a.PatientId, string.Empty);
            return _database.For<BookedSlot>().UpdateOneAsync(filter, update);
        }

        public async Task<BookedSlot> GetSlot(string slotId)
        {
            return (await _database.For<BookedSlot>().FindAsync(x => x.Id == slotId)).FirstOrDefault();

        }
    }
}
