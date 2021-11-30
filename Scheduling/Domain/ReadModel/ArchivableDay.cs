using System;
using Scheduling.Infrastructure.MongoDb;

namespace Scheduling.Domain.ReadModel
{
    public record ArchivableDay(
        string Id,
        DateTime Date
    );
}
