using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Infrastructure.Commands
{
    public class CommandHandler
    {
        internal Dictionary<string, Func<object, CommandMetadata, Task>> Handlers { get; } =
            new Dictionary<string, Func<object, CommandMetadata, Task>>();

        protected void Register<T>(Func<T, CommandMetadata, Task> handler) =>
            Handlers.Add(typeof(T).Name, (c, m) => handler((T) c, m));
    }
}
