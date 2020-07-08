using System.Threading.Tasks;

namespace Scheduling.Domain.Infrastructure.Projections
{
    public interface ISubscription
    {
        Task Project(object @event);
    }
}