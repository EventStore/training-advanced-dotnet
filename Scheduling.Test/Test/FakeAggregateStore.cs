using System.Threading.Tasks;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Test.Test
{
    public class FakeAggregateStore : IAggregateStore
    {
        private readonly AggregateRoot _aggregateRoot;

        public FakeAggregateStore(AggregateRoot aggregateRoot)
        {
            _aggregateRoot = aggregateRoot;
        }

        public Task Save<T>(T aggregate, CommandMetadata metadata) where T : AggregateRoot
        {
            return Task.CompletedTask;
        }

        public Task<T> Load<T>(string aggregateId) where T : AggregateRoot
        {
            return Task.FromResult((T) _aggregateRoot);
        }
    }
}
