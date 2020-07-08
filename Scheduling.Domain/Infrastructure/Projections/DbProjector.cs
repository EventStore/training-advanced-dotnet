using System.Threading.Tasks;

namespace Scheduling.Domain.Infrastructure.Projections
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

        public async Task Project(object @event)
        {
            if (!_eventHandler.CanHandle(@event.GetType())) return;

            await _eventHandler.Handle(@event.GetType(), @event);
        }
    }
}
