using System.Collections.Generic;

namespace Scheduling.Domain.EventSourcing
{
    public interface IColdStorage
    {
        void SaveAll(IEnumerable<object> events);
    }
}
