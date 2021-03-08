using System.Threading.Tasks;
using Scheduling.EventSourcing;

namespace Scheduling.Infrastructure.Projections
{
    public class DbProjector: ISubscription
    {
        readonly EventHandler _eventHandler;

        public DbProjector(
            EventHandler eventHandler
        )
        {
            _eventHandler = eventHandler;
        }

        public async Task Project(object @event, EventMetadata metadata)
        {
            if (!_eventHandler.CanHandle(@event.GetType())) return;

            await _eventHandler.Handle(@event.GetType(), @event, metadata);
        }
    }
}
