using Scheduling.Infrastructure.MongoDb;

namespace Scheduling.Domain.Domain.ReadModel
{
    public class BookedSlot : Document
    {
        public string DayId { get; set; }

        public int Month { get; set; }

        public string PatientId { get; set; }

        public bool IsBooked { get; set; }
    }
}
