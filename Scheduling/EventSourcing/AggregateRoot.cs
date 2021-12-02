using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling.EventSourcing;

public abstract class AggregateRoot
{
    private readonly Dictionary<Type, Action<object>> _handlers = new();

    private readonly List<object> _changes = new();

    public string Id { get; set; } = default!;

    public int Version { get; protected set; } = -1;

    protected void Register<T>(Action<T> when) =>
        _handlers.Add(typeof(T), e => when((T) e));

    protected void Raise(object e)
    {
        _handlers[e.GetType()](e);
        _changes.Add(e);
    }

    public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

    public void Load(IEnumerable<object> history)
    {
        foreach (var e in history)
        {
            Raise(e);
            Version++;
        }
    }

    public void ClearChanges() => _changes.Clear();
}