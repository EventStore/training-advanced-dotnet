using System;
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

services.AddSingleton<IEventStore>(eventStore);
services.AddSingleton(Helpers.GetDispatcher(eventStore));
services.AddSingleton(_ => mongoClient.GetDatabase("projections"));
services.AddSingleton<IAvailableSlotsRepository, MongoDbAvailableSlotsRepository>();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
            { Title = "Patients API", Description = "API for booking doctor appointments", Version = "v1" });
});

EventMappings.MapEventTypes();

var mongoDatabase = mongoClient.GetDatabase("projections");
var availableSlotsRepository = new MongoDbAvailableSlotsRepository(mongoDatabase);

var dispatcher = Helpers.GetDispatcher(eventStore);

var commandStore = new EsCommandStore(eventStore, client, dispatcher, Helpers.Tenant);

var dayArchiverProcessManager = new DayArchiverProcessManager(
    new InMemoryColdStorage(),
    new MongoDbArchivableDaysRepository(mongoDatabase),
    commandStore,
    TimeSpan.FromDays(-180),
    eventStore,
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

await subManager.Start();
await commandStore.Start();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseSwagger();

app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patients API"); });

app.Run();