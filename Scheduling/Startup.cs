using System;
using System.Collections.Generic;
using System.Net.Http;
using EventStore.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Scheduling.Domain.ReadModel;
using Scheduling.EventSourcing;
using Scheduling.Infrastructure.EventStore;
using Scheduling.Infrastructure.MongoDb;

namespace Scheduling
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            var mongoClient = new MongoClient("mongodb://localhost");

            var client = Helpers.GetEventStoreClient();
            var eventStore = new EsEventStore(client, Helpers.Tenant);

            services.AddSingleton<IEventStore>(eventStore);

            services.AddSingleton(Helpers.GetDispatcher(eventStore));

            services.AddSingleton(c => mongoClient.GetDatabase("projections"));
            services.AddSingleton<IAvailableSlotsRepository, MongoDbAvailableSlotsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public class ClassifiedAdDetails
    {
        public string Id { get; set; }
        public Guid SellerId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }

        public List<string> PhotoUrls { get; set; }
            = new List<string>();

        public static string GetDatabaseId(Guid id)
            => $"ClassifiedAdDetails/{id}";
    }
}
