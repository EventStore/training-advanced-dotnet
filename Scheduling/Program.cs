using System;
using System.Net.Http;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Scheduling.Domain.Application;
using Scheduling.Domain.Domain.DoctorDay.Events;
using Scheduling.Domain.Domain.ReadModel;
using Scheduling.Domain.Domain.Service;
using Scheduling.Domain.EventSourcing;
using Scheduling.Domain.Infrastructure.Commands;
using Scheduling.Domain.Infrastructure.EventStore;
using Scheduling.Domain.Infrastructure.InMemory;
using Scheduling.Domain.Infrastructure.MongoDb;
using Scheduling.Domain.Infrastructure.Projections;

namespace Scheduling
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            EventMappings.MapEventTypes();

            var client = Helpers.GetEventStoreClient();
            var esStore = new EsEventStore(client, Helpers.Tenant);

            var mongoClient = new MongoClient("mongodb://localhost");

            var mongoDatabase = mongoClient.GetDatabase("projections");
            var availableSlotsRepository = new MongoDbAvailableSlotsRepository(mongoDatabase);

            var dispatcher = Helpers.GetDispatcher(esStore);

            var commandStore = new EsCommandStore(esStore, client, dispatcher, Helpers.Tenant);

            var dayArchiverProcessManager = new DayArchiverProcessManager(
                new InMemoryColdStorage(),
                new MongoDbArchivableDaysRepository(mongoDatabase),
                commandStore,
                TimeSpan.FromDays(-180),
                esStore,
                Guid.NewGuid);

            var subManager = new SubscriptionManager(
                client,
                new EsCheckpointStore(client, "DaySubscription"),
                "DaySubscription",
                StreamName.AllStream,
                new DbProjector(
                    new AvailableSlotsProjection(availableSlotsRepository)
                ),
                new DbProjector(
                    dayArchiverProcessManager)
            );

            await subManager.Start();
            await commandStore.Start();

            CreateHostBuilder(args).Build().Run();
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
