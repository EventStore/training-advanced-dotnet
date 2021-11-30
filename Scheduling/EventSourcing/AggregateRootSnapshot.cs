using System;

namespace Scheduling.EventSourcing
{
    public abstract class AggregateRootSnapshot : AggregateRoot
    {
        private Action<object> _snapshotLoad;

        private Func<object> _snapshotGet;

        public int SnapshotVersion { get; set; }

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
}