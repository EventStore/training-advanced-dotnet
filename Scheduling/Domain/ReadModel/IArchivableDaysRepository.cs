using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scheduling.Domain.ReadModel
{
    public interface IArchivableDaysRepository
    {
        Task Add(ArchivableDay archivableDay);

        Task<List<ArchivableDay>> FindAll(DateTime dateThreshold);
    }
}
