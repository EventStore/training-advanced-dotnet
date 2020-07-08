using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduling.Infrastructure.Projections
{
    public class EventHandler
    {
        readonly List<EventHandlerEnvelope> _handlers = new List<EventHandlerEnvelope>();

        protected void When<T>(Func<T, Task> when)
        {
            _handlers.Add(new EventHandlerEnvelope { EventType = typeof(T), Handler = async e => await when((T)e) });
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

        public class EventHandlerEnvelope
        {
            public Type EventType { get; set; }

            public Func<object, Task> Handler { get; set; }
        }
    }
}
