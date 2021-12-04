using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.EventSourcing;

namespace Scheduling.Infrastructure.Commands;

public class CommandHandlerMap
{
    private readonly Dictionary<string, Func<object, CommandMetadata, Task>> _handlers =
        new Dictionary<string, Func<object, CommandMetadata, Task>>();

    public CommandHandlerMap(params CommandHandler[] commandHandlers)
    {
        foreach (var handler in commandHandlers.SelectMany(h => h.Handlers))
        {
            if (!_handlers.TryAdd(handler.Key, handler.Value))
            {
                throw new DuplicateCommandHandlerException(handler.Key);
            }
        }
    }

    public Func<object, CommandMetadata, Task>? Get(object command) =>
        _handlers.ContainsKey(command.GetType().Name) ? _handlers[command.GetType().Name] : null;
}

public class DuplicateCommandHandlerException : Exception
{
    public DuplicateCommandHandlerException(string type) :
        base($"A handler has already been registered for {type}")
    {
    }
}