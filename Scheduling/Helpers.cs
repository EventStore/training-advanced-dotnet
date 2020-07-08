using System;
using System.Net.Http;
using EventStore.Client;
using Scheduling.Domain.Domain.Service;
using Scheduling.Domain.Infrastructure.Commands;
using Scheduling.Domain.Infrastructure.EventStore;

namespace Scheduling
{
    public static class Helpers
    {
        public static string Tenant { get; } = "Scheduling";

        public static EventStoreClient GetEventStoreClient() =>
            new EventStoreClient(new EventStoreClientSettings
            {
                ConnectivitySettings =
                {
                    Address = new Uri("https://localhost:2113"),
                },
                DefaultCredentials = new UserCredentials("admin", "changeit"),
                CreateHttpMessageHandler = () =>
                    new SocketsHttpHandler
                    {
                        SslOptions =
                        {
                            RemoteCertificateValidationCallback = delegate { return true; }
                        }
                    }
            });

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
