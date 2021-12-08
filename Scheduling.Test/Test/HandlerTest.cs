using System;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.EventSourcing;
using Xunit;
using EventHandler = Scheduling.Infrastructure.Projections.EventHandler;

namespace Scheduling.Test.Test;

public abstract class HandlerTest
{
    protected abstract EventHandler GetHandler();

    protected bool EnableAtLeastOnceMonkey { get; set; }

    private EventHandler _eventHandler = default!;

    protected async Task Given(params object[] events)
    {
        _eventHandler = GetHandler();

        var correlationId = new CorrelationId(Guid.NewGuid());
        var causationId = new CausationId(Guid.NewGuid());

        ulong i = 0;
        foreach (var @event in events)
        {
            var metadata = new EventMetadata(
                correlationId,
                causationId,
                i
            );

            await _eventHandler.Handle(@event.GetType(), @event, metadata);

            if (EnableAtLeastOnceMonkey)
                await _eventHandler.Handle(@event.GetType(), @event, metadata);

            i++;
        }

        if (EnableAtLeastOnceMonkey)
        {
            i = 0;
            foreach (var @event in events.Take(events.Length - 1))
            {
                var metadata = new EventMetadata(
                    correlationId,
                    causationId,
                    i
                );

                await _eventHandler.Handle(@event.GetType(), @event, metadata);
                i++;
            }
        }
    }

    protected void Then(object expected, object actual)
    {
        Assert.Equal(expected, actual);
    }
}