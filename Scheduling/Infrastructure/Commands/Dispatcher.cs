using System;
using System.Threading.Tasks;
using Scheduling.EventSourcing;

namespace Scheduling.Infrastructure.Commands;

public class Dispatcher
{
    private readonly CommandHandlerMap _map;

    public Dispatcher(CommandHandlerMap map) =>
        _map = map;

    public Task Dispatch(object command, CommandMetadata metadata)
    {
        var handler = _map.Get(command);

        if (handler == null)
        {
            throw new HandlerNotFoundException(command);
        }

        return handler(command, metadata);
    }
}

public class HandlerNotFoundException : Exception
{
    public HandlerNotFoundException(object type) :
        base($"No handler found for {type.GetType().Name}")
    {
    }
}