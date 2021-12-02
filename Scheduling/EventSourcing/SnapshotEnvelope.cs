namespace Scheduling.EventSourcing;

public record SnapshotEnvelope(
    object Snapshot,
    SnapshotMetadata Metadata
);