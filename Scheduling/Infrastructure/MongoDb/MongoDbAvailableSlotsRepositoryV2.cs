using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb
{
    public class MongoDbAvailableSlotsRepositoryV2 : IAvailableSlotsRepository
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<MongoDbAvailableSlotV2> _collection;

        public MongoDbAvailableSlotsRepositoryV2(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<MongoDbAvailableSlotV2>("available_slots_v2");
        }
        public async Task<List<AvailableSlot>> GetAvailableSlotsOn(DateTime today)
        {
            var filter = Builders<MongoDbAvailableSlotV2>.Filter.Where(x => x.Date == today.Date.ToString("dd-MM-yyyy") && x.IsBooked == false);
            var availableSlots = await _collection.FindAsync(filter);
            return availableSlots.ToList().Select(s => s.ToAvailableSlot()).ToList();
        }

        public Task AddSlot(MongoDbAvailableSlotV2 availableSlot)
        {
            return _collection.InsertOneAsync(availableSlot);
        }

        public Task HideSlot(Guid slotId)
        {
            var filter = Builders<MongoDbAvailableSlotV2>.Filter.Eq(x => x.Id, slotId.ToString());
            var update = Builders<MongoDbAvailableSlotV2>.Update.Set(a => a.IsBooked, true);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task ShowSlot(Guid slotId)
        {
            var filter = Builders<MongoDbAvailableSlotV2>.Filter.Eq(x => x.Id, slotId.ToString());
            var update = Builders<MongoDbAvailableSlotV2>.Update.Set(a => a.IsBooked, false);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteSlot(Guid slotId)
        {
            var filter = Builders<MongoDbAvailableSlotV2>.Filter.Eq(x => x.Id, slotId.ToString());
            return _collection.FindOneAndDeleteAsync(filter);
        }
    }
}
