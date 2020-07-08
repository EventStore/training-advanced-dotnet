namespace Scheduling.Domain.EventSourcing
{
    public class SnapshotMetadata
    {
        public string ClrType { get; set; }

        public string CorrelationId { get; set; }

        public string CausationId { get; set; }

        public int Version { get; set; }
    }
}
