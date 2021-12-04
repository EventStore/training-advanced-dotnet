using System.Threading.Tasks;

namespace Scheduling.EventSourcing;

public interface IAggregateStore
{
    Task Save<T>(T aggregate, CommandMetadata metadata) where T : AggregateRoot;

    Task<T> Load<T>(string aggregateId) where T : AggregateRoot;
}