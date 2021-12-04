using System;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Scheduling;
using Scheduling.Application;
using Scheduling.Domain.DoctorDay.Events;
using Scheduling.Domain.ReadModel;
using Scheduling.EventSourcing;
using Scheduling.Infrastructure.EventStore;
using Scheduling.Infrastructure.InMemory;
using Scheduling.Infrastructure.MongoDb;
using Scheduling.Infrastructure.Projections;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers().AddNewtonsoftJson();

var mongoClient = new MongoClient("mongodb://localhost");

var client = Helpers.GetEventStoreClient();
var eventStore = new EsEventStore(client, Helpers.Tenant);

services.AddSingleton<IEventStore>(eventStore)
    .AddSingleton(Helpers.GetDispatcher(eventStore))
    .AddSingleton(_ => mongoClient.GetDatabase("projections"))
    .AddSingleton<IAvailableSlotsRepository, MongoDbAvailableSlotsRepository>()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Patients API", Description = "API for booking doctor appointments", Version = "v1" });
    });

EventMappings.MapEventTypes();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection()
    .UseRouting()
    .UseAuthorization()
    .UseEndpoints(endpoints => { endpoints.MapControllers(); })
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patients API");
    });

await StartSubscriptionManager(mongoClient, eventStore, client);
app.Run();

async Task StartSubscriptionManager(MongoClient mongoClient1, EsEventStore esEventStore,
    EventStoreClient eventStoreClient)
{
    var mongoDatabase = mongoClient1.GetDatabase("projections");
    var availableSlotsRepository = new MongoDbAvailableSlotsRepository(mongoDatabase);

    var dispatcher = Helpers.GetDispatcher(esEventStore);

    var commandStore = new EsCommandStore(esEventStore, eventStoreClient, dispatcher, Helpers.Tenant);

    var dayArchiverProcessManager = new DayArchiverProcessManager(
        new InMemoryColdStorage(),
        new MongoDbArchivableDaysRepository(mongoDatabase),
        commandStore,
        TimeSpan.FromDays(-180),
        esEventStore,
        Guid.NewGuid);

    var subManager = new SubscriptionManager(
        eventStoreClient,
        new EsCheckpointStore(eventStoreClient, "DaySubscription"),
        "DaySubscription",
        StreamName.AllStream,
        new Projector(
            new AvailableSlotsProjection(availableSlotsRepository)
        ),
        new Projector(
            dayArchiverProcessManager)
    );

    await subManager.Start();
    await commandStore.Start();
}
