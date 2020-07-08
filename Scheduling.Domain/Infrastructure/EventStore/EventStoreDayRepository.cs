using System.Threading.Tasks;
using Scheduling.Domain.Domain.DoctorDay;
using Scheduling.Domain.Domain.Service;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Infrastructure.EventStore
{
    public class EventStoreDayRepository : IDayRepository
     {
         private readonly IAggregateStore _aggregateStore;

         public EventStoreDayRepository(IAggregateStore aggregateStore)
         {
             _aggregateStore = aggregateStore;
         }

         public Task Save(Day day, CommandMetadata metadata)
         {
             return _aggregateStore.Save(day, metadata);
         }

         public Task<Day> Get(DayId dayId)
         {
             return _aggregateStore.Load<Day>(dayId.Value);
         }
     }
}
