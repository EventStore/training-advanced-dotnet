using System.Threading.Tasks;

namespace Scheduling.Domain.Infrastructure.Projections
{
    public interface ICheckpointStore
    {
        Task<ulong?> GetCheckpoint();

        Task StoreCheckpoint(ulong? checkpoint);
    }
}
