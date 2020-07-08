// using System;
// using System.Collections.Generic;
// using System.Linq.Expressions;
// using System.Threading;
// using System.Threading.Tasks;
// using Raven.Client.Documents.Indexes;
// using Raven.Client.Documents.Linq;
// using Raven.Client.Documents.Session;
// using Raven.Client.Documents.Session.Loaders;
// using Scheduling.Domain.Domain.Service.Projections;
// using Scheduling.Domain.Infrastructure.Projections;
// using Xunit;
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Infrastructure.InMemory;
using MongoDB.Driver;
using Scheduling.Domain.Application;
using Scheduling.Domain.Domain.DoctorDay.Events;
using Scheduling.Domain.Domain.ReadModel;
using Scheduling.Domain.Infrastructure.MongoDb;
using Scheduling.Test.Test;
using Xunit;
using EventHandler = Scheduling.Domain.Infrastructure.Projections.EventHandler;

namespace Scheduling.Test
{
    public class AvailableSlotsHandlerTest : HandlerTest
    {
        private static InMemoryAvailableSlotsRepository _repository;

        private readonly DateTime _now = DateTime.UtcNow;

        private TimeSpan _tenMinutes = TimeSpan.FromMinutes(10);

        protected override EventHandler GetHandler()
        {
            // var mongoClient = new MongoClient("mongodb://localhost");
            // _repository = new MongoDbAvailableSlotsRepository(mongoClient.GetDatabase(Guid.NewGuid().ToString()));
            _repository = new InMemoryAvailableSlotsRepository();
            _repository.Clear();
            return new AvailableSlotsProjection(_repository);
        }

        [Fact]
        public async Task should_add_slot_to_the_list()
        {
            var scheduled = new SlotScheduled(Guid.NewGuid(), "dayId", _now, _tenMinutes);
            await Given(scheduled);
            Then(new List<AvailableSlot>
            {
                new AvailableSlot
                {
                    Date = scheduled.SlotStartTime.Date,
                    Duration = scheduled.SlotDuration,
                    Id = scheduled.SlotId.ToString(),
                    DayId = scheduled.DayId,
                    IsBooked = false,
                    StartTime = scheduled.SlotStartTime
                }
            }, await _repository.GetAvailableSlotsOn(_now));
        }

        [Fact]
        public async Task should_hide_the_slot_from_list_if_booked()
        {
            var scheduled = new SlotScheduled(Guid.NewGuid(), "dayId", _now, _tenMinutes);
            Given(
                scheduled,
                new SlotBooked("dayId", scheduled.SlotId, "PatientId"));
            Then(new List<AvailableSlot>(), await _repository.GetAvailableSlotsOn(_now));
        }

        [Fact]
        public async Task should_show_slot_if_booking_was_cancelled()
        {
            var scheduled = new SlotScheduled(Guid.NewGuid(), "dayId", _now, _tenMinutes);
            Given(
                scheduled,
                new SlotBooked("dayId", scheduled.SlotId, "PatientId"),
                new SlotBookingCancelled("dayId", scheduled.SlotId, "Reason"));
            Then(new List<AvailableSlot>
            {
                new AvailableSlot
                {
                    Date = scheduled.SlotStartTime.Date,
                    Duration = scheduled.SlotDuration,
                    Id = scheduled.SlotId.ToString(),
                    DayId = scheduled.DayId,
                    IsBooked = false,
                    StartTime = scheduled.SlotStartTime
                }
            }, await _repository.GetAvailableSlotsOn(_now));
        }

        [Fact]
        public async Task should_delete_slot_if_slot_was_cancelled()
        {
            var scheduled = new SlotScheduled(Guid.NewGuid(), "dayId", _now, _tenMinutes);
            Given(
                scheduled,
                new SlotScheduleCancelled("dayId", scheduled.SlotId));
            Then(new List<AvailableSlot>(), await _repository.GetAvailableSlotsOn(_now));
        }
    }
}
//
//     public class ProjectionTest<TProjection, TProjectionState> where TProjection : Projection
//     {
//         private TProjection _projection;
//
//         public ProjectionTest()
//         {
//             _projection = (TProjection) Activator.CreateInstance(typeof(TProjection));
//         }
//
//         public void Given(params object[] @events)
//         {
//             foreach (var @event in events)
//             {
//                 _projection.Handle(@event.GetType(), @event, new FakeRepo<TProjectionState>());
//             }
//         }
//     }
//
//     public class FakeRepo : IAsyncDocumentSession
//     {
//         private readonly Dictionary<string, object> _entities = new Dictionary<string, object>();
//
//         public FakeRepo()
//         {
//
//         }
//
//         public void Dispose()
//         {
//         }
//
//         public IAsyncSessionDocumentCounters CountersFor(string documentId)
//         {
//         }
//
//         public IAsyncSessionDocumentCounters CountersFor(object entity)
//         {
//         }
//
//         public void Delete<T>(T entity)
//         {
//             _entities.
//         }
//
//         public void Delete(string id)
//         {
//         }
//
//         public void Delete(string id, string expectedChangeVector)
//         {
//         }
//
//         public Task SaveChangesAsync(CancellationToken token = new CancellationToken())
//         {
//         }
//
//         public Task StoreAsync(object entity, CancellationToken token = new CancellationToken())
//         {
//         }
//
//         public Task StoreAsync(object entity, string changeVector, string id,
//             CancellationToken token = new CancellationToken())
//         {
//         }
//
//         public Task StoreAsync(object entity, string id, CancellationToken token = new CancellationToken())
//         {
//         }
//
//         public IAsyncLoaderWithInclude<object> Include(string path)
//         {
//         }
//
//         public IAsyncLoaderWithInclude<T> Include<T>(Expression<Func<T, string>> path)
//         {
//         }
//
//         public IAsyncLoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, string>> path)
//         {
//         }
//
//         public IAsyncLoaderWithInclude<T> Include<T>(Expression<Func<T, IEnumerable<string>>> path)
//         {
//         }
//
//         public IAsyncLoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, IEnumerable<string>>> path)
//         {
//         }
//
//         public Task<T> LoadAsync<T>(string id, CancellationToken token = new CancellationToken())
//         {
//         }
//
//         public Task<Dictionary<string, T>> LoadAsync<T>(IEnumerable<string> ids,
//             CancellationToken token = new CancellationToken())
//         {
//         }
//
//         public Task<T> LoadAsync<T>(string id, Action<IIncludeBuilder<T>> includes,
//             CancellationToken token = new CancellationToken())
//         {
//         }
//
//         public Task<Dictionary<string, T>> LoadAsync<T>(IEnumerable<string> ids, Action<IIncludeBuilder<T>> includes,
//             CancellationToken token = new CancellationToken())
//         {
//         }
//
//         public IRavenQueryable<T> Query<T>(string indexName = null, string collectionName = null,
//             bool isMapReduce = false)
//         {
//         }
//
//         public IRavenQueryable<T> Query<T, TIndexCreator>() where TIndexCreator : AbstractIndexCreationTask, new()
//         {
//         }
//
//         public IAsyncAdvancedSessionOperations Advanced { get; }
//     }
// }
