using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Infrastructure.MongoDb;

public class MongoDbArchivableDaysRepository : IArchivableDaysRepository
{
        private readonly IMongoDatabase _database;

        public MongoDbArchivableDaysRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public Task Add(ArchivableDay archivableDay)
        {
            return _database.For<ArchivableDay>().InsertOneAsync(archivableDay);
        }

        public async Task<List<ArchivableDay>> FindAll(DateTime dateThreshold)
        {
            var filter = Builders<ArchivableDay>.Filter.Where(x => x.Date <= dateThreshold);
            var availableSlots = await _database.For<ArchivableDay>().FindAsync(filter);
            return availableSlots.ToList();
        }
}