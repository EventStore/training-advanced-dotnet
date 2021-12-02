using System;

namespace Scheduling.EventSourcing;

public abstract class AggregateRootSnapshot : AggregateRoot
{
    private Action<object> _snapshotLoad = default!;

    private Func<object> _snapshotGet = default!;

    public int SnapshotVersion { get; private set; }

    protected void RegisterSnapshot<T>(Action<T> load, Func<object> get)
    {
        _snapshotLoad = e => load((T)e);
        _snapshotGet = get;
    }

    public void LoadSnapshot(object snapshot, int version)
    {
        _snapshotLoad(snapshot);
        Version = version;
        SnapshotVersion = version;
    }

    public object GetSnapshot() =>
        _snapshotGet();
}