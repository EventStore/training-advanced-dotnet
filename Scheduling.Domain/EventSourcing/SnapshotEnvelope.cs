namespace Scheduling.Domain.EventSourcing
{
    public class SnapshotEnvelope
    {
        public object Snapshot { get; set; }

        public SnapshotMetadata Metadata { get; set; }
    }
}
