using System;
using Scheduling.Infrastructure.MongoDb;

namespace Scheduling.Domain.ReadModel
{
    public class ArchivableDay : Document
    {
        public DateTime Date { get; set; }
    }
}
