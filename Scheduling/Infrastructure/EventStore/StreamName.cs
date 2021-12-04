namespace Scheduling.Infrastructure.EventStore;

public class StreamName
{
    private string Value { get; }
    const string AllStreamName = "$all";

    private StreamName(string value) => Value = value;

    public static StreamName AllStream => new StreamName(AllStreamName);

    public static StreamName For<T>(string id)
        => new StreamName($"{typeof(T).Name}-{id}");

    public static StreamName Custom(string streamName)
        => new StreamName(streamName);

    public bool IsAllStream => Value.Equals(AllStreamName);

    public override string ToString() => Value ?? "";

    public static implicit operator string(StreamName self)
        => self.ToString();
}