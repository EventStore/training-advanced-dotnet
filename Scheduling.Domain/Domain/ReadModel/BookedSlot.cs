using Scheduling.Domain.Infrastructure.MongoDb;

namespace Scheduling.Domain.Domain.ReadModel
{
    public class BookedSlot : Document<BookedSlot>
    {
        public string DayId { get; set; }

        public int Month { get; set; }

        public string PatientId { get; set; }
    }
}
