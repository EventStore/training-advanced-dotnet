using System.Collections.Generic;

namespace Scheduling.EventSourcing;

public interface IColdStorage
{
    void SaveAll(IEnumerable<object> events);
}