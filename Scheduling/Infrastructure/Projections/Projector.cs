using System.Threading.Tasks;
using Scheduling.EventSourcing;

namespace Scheduling.Infrastructure.Projections
{
    public class Projector: ISubscription
    {
        readonly EventHandler _eventHandler;

        public Projector(
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
