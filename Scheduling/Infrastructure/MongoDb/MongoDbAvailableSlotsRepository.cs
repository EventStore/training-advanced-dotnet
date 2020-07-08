using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb
{
    public class MongoDbAvailableSlotsRepository : IAvailableSlotsRepository
    {
        private readonly IMongoDatabase _database;

        public MongoDbAvailableSlotsRepository(IMongoDatabase database)
        {
            _database = database;
        }
        public async Task<List<AvailableSlot>> GetAvailableSlotsOn(DateTime today)
        {
            var filter = Builders<AvailableSlot>.Filter.Where(x => x.Date == today.Date && x.IsBooked == false);
            var availableSlots = await _database.For<AvailableSlot>().FindAsync(filter);
            return availableSlots.ToList();
        }

        public Task AddSlot(AvailableSlot availableSlot)
        {
            return _database.For<AvailableSlot>().InsertOneAsync(availableSlot);
        }

        public Task HideSlot(Guid slotId)
        {
            var filter = Builders<AvailableSlot>.Filter.Eq(x => x.Id, slotId.ToString());
            var update = Builders<AvailableSlot>.Update.Set(a => a.IsBooked, true);
            return _database.For<AvailableSlot>().UpdateOneAsync(filter, update);
        }

        public Task ShowSlot(Guid slotId)
        {
            var filter = Builders<AvailableSlot>.Filter.Eq(x => x.Id, slotId.ToString());
            var update = Builders<AvailableSlot>.Update.Set(a => a.IsBooked, false);
            return _database.For<AvailableSlot>().UpdateOneAsync(filter, update);
        }

        public Task DeleteSlot(Guid slotId)
        {
            var filter = Builders<AvailableSlot>.Filter.Eq(x => x.Id, slotId.ToString());
            return _database.For<AvailableSlot>().FindOneAndDeleteAsync(filter);
        }
    }
}
