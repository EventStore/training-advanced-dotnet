using System;
using Scheduling.Domain.Infrastructure.MongoDb;

namespace Scheduling.Domain.Domain.ReadModel
{
    public class ArchivableDay : Document<ArchivableDay>
    {
        public DateTime Date { get; set; }
    }
}
