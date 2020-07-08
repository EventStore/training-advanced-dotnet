namespace Scheduling.Domain.EventSourcing
{
    public class Event<T> : Value<T> where T : Value<T>
    {

    }
}
