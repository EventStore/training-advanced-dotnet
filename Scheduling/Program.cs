using System;
using System.Net.Http;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Scheduling.Application;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Infrastructure.EventStore;
using Scheduling.Infrastructure.InMemory;
using Scheduling.Infrastructure.MongoDb;
using Scheduling.Infrastructure.Projections;

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
                new Projector(
                    new AvailableSlotsProjection(availableSlotsRepository)
                ),
                new Projector(
                    dayArchiverProcessManager)
            );

            Task.Run(() => subManager.Start());
            await commandStore.Start();

            CreateHostBuilder(args).Build().Run();
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
