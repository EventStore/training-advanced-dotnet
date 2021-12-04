using System.Collections.Generic;
using Scheduling.EventSourcing;

namespace Scheduling.Infrastructure.InMemory;

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