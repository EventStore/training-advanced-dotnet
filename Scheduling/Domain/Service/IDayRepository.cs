using System.Threading.Tasks;
using Scheduling.Domain.DoctorDay;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.Service;

public interface IDayRepository
{
    Task Save(Day day, CommandMetadata metadata);

    Task<Day> Get(DayId dayId);
}