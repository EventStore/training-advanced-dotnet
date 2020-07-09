using System;
using Scheduling.Infrastructure.MongoDb;

namespace Scheduling.Domain.ReadModel
{
    public class ArchivableDay
    {
        public DateTime Date { get; set; }
        
        public string Id { get; set; }
    }
}
