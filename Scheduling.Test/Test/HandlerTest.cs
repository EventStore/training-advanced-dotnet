using System;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.EventSourcing;
using Xunit;
using EventHandler = Scheduling.Infrastructure.Projections.EventHandler;

namespace Scheduling.Test.Test
{
    public abstract class HandlerTest
    {
        protected abstract EventHandler GetHandler();

        protected bool EnableAtLeastOnceMonkey { get; set; }

        private EventHandler _eventHandler;

        protected async Task Given(params object[] events)
        {
            _eventHandler = GetHandler();

            var metadata = new EventMetadata
                {CausationId = new CausationId(Guid.NewGuid()), CorrelationId = new CorrelationId(Guid.NewGuid())};

            foreach (var @event in events)
            {
                await _eventHandler.Handle(@event.GetType(), @event, metadata);

                if (EnableAtLeastOnceMonkey)
                    await _eventHandler.Handle(@event.GetType(), @event, metadata);
            }

            if (EnableAtLeastOnceMonkey)
            {
                foreach (var @event in events.Take(events.Length - 1))
                {
                    await _eventHandler.Handle(@event.GetType(), @event, metadata);
                }
            }
        }

        protected void Then(object expected, object actual)
        {
            Assert.Equal(expected, actual);
        }
    }
}
