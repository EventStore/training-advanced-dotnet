using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.Domain.ReadModel;

namespace Scheduling.Infrastructure.InMemory
{
    public class InMemoryArchivableDaysRepository : IArchivableDaysRepository
    {
        private List<ArchivableDay> _archivableDays = new List<ArchivableDay>();

        public Task Add(ArchivableDay archivableDay)
        {
            _archivableDays.Add(archivableDay);
            return Task.CompletedTask;
        }

        public Task<List<ArchivableDay>> FindAll(DateTime dateThreshold)
        {
            return Task.FromResult(_archivableDays.Where(d => d.Date <= dateThreshold).ToList());
        }
    }
}
