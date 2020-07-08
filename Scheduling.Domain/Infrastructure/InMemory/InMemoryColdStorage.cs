using System.Collections.Generic;
using System.Linq;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Infrastructure.InMemory
{
    public class InMemoryColdStorage : IColdStorage
    {
        public List<object> Events { get; } = new List<object>();

        public void SaveAll(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                Events.Add(@event);
            }
        }
    }
}
