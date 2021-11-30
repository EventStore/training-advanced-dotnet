using EventStore.Client;
using Scheduling.Domain.Service;
using Scheduling.Infrastructure.Commands;
using Scheduling.Infrastructure.EventStore;

namespace Scheduling
{
    public static class Helpers
    {
        public static string Tenant { get; } = "Scheduling";

        public static EventStoreClient GetEventStoreClient() =>
            new(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false"));

        public static Dispatcher GetDispatcher(EsEventStore esStore)
        {
            var aggregateStore = new EsAggregateStore(esStore, 5);
            var dayRepository = new EventStoreDayRepository(aggregateStore);

            var handlers = new Handlers(dayRepository);

            var commandHandlerMap = new CommandHandlerMap(handlers);

            var dispatcher = new Dispatcher(commandHandlerMap);
            return dispatcher;
        }
    }
}
