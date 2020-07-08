using System;
using Scheduling.Domain.Infrastructure.MongoDb;

namespace Scheduling.Domain.Domain.ReadModel
{
    public class ArchivableDay : Document
    {
        public DateTime Date { get; set; }
    }
}
