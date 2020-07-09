using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb
{
    public class MongoDbAvailableSlotsRepository : IAvailableSlotsRepository
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<MongoDbAvailableSlot> _collection;

        public MongoDbAvailableSlotsRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<MongoDbAvailableSlot>("available_slots");
        }
        public async Task<List<AvailableSlot>> GetAvailableSlotsOn(DateTime today)
        {
            var filter = Builders<MongoDbAvailableSlot>.Filter.Where(x => x.Date == today.Date.ToString("dd-MM-yyyy") && x.IsBooked == false);
            var availableSlots = await _collection.FindAsync(filter);
            return availableSlots.ToList().Cast<AvailableSlot>().ToList();;
        }

        public Task AddSlot(MongoDbAvailableSlot availableSlot)
        {
            return _collection.InsertOneAsync(availableSlot);
        }

        public Task HideSlot(Guid slotId)
        {
            var filter = Builders<MongoDbAvailableSlot>.Filter.Eq(x => x.Id, slotId.ToString());
            var update = Builders<MongoDbAvailableSlot>.Update.Set(a => a.IsBooked, true);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task ShowSlot(Guid slotId)
        {
            var filter = Builders<MongoDbAvailableSlot>.Filter.Eq(x => x.Id, slotId.ToString());
            var update = Builders<MongoDbAvailableSlot>.Update.Set(a => a.IsBooked, false);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteSlot(Guid slotId)
        {
            var filter = Builders<MongoDbAvailableSlot>.Filter.Eq(x => x.Id, slotId.ToString());
            return _collection.FindOneAndDeleteAsync(filter);
        }
    }
}
