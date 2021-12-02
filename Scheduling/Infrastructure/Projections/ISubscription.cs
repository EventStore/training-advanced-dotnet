using System.Threading.Tasks;

namespace Scheduling.Infrastructure.Projections;

public interface ISubscription
{
    Task Project(object @event);
}