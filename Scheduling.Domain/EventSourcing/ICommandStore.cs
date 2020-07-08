using System.Threading.Tasks;

namespace Scheduling.Domain.EventSourcing
{
    public interface ICommandStore
    {
        Task Send(object command, CommandMetadata metadata);

        public Task Start();
    }
}
