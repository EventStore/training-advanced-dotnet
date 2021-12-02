using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduling.Infrastructure.Projections
{
    public class EventHandler
    {
        readonly List<EventHandlerEnvelope> _handlers = new();

        protected void When<T>(Func<T, Task> when)
        {
            _handlers.Add(new EventHandlerEnvelope(typeof(T), async (e) => await when((T)e)));
        }

        public async Task Handle(Type eventType, object e)
        {
            var handlers = _handlers
                .Where(h => h.EventType == eventType)
                .ToList();

            foreach (var handler in handlers)
            {
                await handler.Handler(e);
            }
        }

        public bool CanHandle(Type eventType)
        {
            return _handlers.Any(h => h.EventType == eventType);
        }

        public record EventHandlerEnvelope(
            Type EventType,
            Func<object, Task> Handler
        );
        }
}
