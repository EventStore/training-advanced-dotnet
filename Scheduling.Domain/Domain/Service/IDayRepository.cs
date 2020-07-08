using System.Threading.Tasks;
using Scheduling.Domain.Domain.DoctorDay;
using Scheduling.Domain.EventSourcing;
using Scheduling.Domain.Infrastructure;

namespace Scheduling.Domain.Domain.Service
{
    public interface IDayRepository
    {
        Task Save(Day day, CommandMetadata metadata);

        Task<Day> Get(DayId dayId);
    }
}
