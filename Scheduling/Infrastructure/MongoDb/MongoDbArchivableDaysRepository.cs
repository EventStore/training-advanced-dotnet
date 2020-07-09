using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb
{
    public class MongoDbArchivableDaysRepository : IArchivableDaysRepository
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<ArchivableDay> _collection;

        public MongoDbArchivableDaysRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<ArchivableDay>("archivable_day");
        }

        public Task Add(ArchivableDay archivableDay)
        {
            return _collection.InsertOneAsync(archivableDay);
        }

        public async Task<List<ArchivableDay>> FindAll(DateTime dateThreshold)
        {
            var filter = Builders<ArchivableDay>.Filter.Where(x => x.Date <= dateThreshold);
            var availableSlots = await _collection.FindAsync(filter);
            return availableSlots.ToList();
        }
    }
}
