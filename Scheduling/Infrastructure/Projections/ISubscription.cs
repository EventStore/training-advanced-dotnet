using System.Threading.Tasks;
using Scheduling.EventSourcing;

namespace Scheduling.Infrastructure.Projections
{
    public interface ISubscription
    {
        Task Project(object @event, EventMetadata metadata);
    }
}
