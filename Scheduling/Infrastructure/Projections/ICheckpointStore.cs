using System.Threading.Tasks;

namespace Scheduling.Infrastructure.Projections
{
    public interface ICheckpointStore
    {
        Task<ulong?> GetCheckpoint();

        Task StoreCheckpoint(ulong? checkpoint);
    }
}
