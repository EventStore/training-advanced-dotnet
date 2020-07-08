using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Client;
using Newtonsoft.Json;
using Scheduling.Domain.Infrastructure.EventStore;

namespace Scheduling.Domain.Infrastructure.Projections
{
    public class EsCheckpointStore : ICheckpointStore
    {
        const string CheckpointStreamPrefix = "checkpoint";
        readonly EventStoreClient _client;
        readonly string _streamName;

        public EsCheckpointStore(
            EventStoreClient client,
            string subscriptionName)
        {
            _client = client;
            _streamName = CheckpointStreamPrefix + subscriptionName;
        }

        public async Task<ulong?> GetCheckpoint()
        {
            var result = _client
                .ReadStreamAsync(Direction.Backwards, _streamName, StreamPosition.End, 1);

            if (await result.ReadState == ReadState.StreamNotFound)
            {
                return null;
            }

            var eventData = await result.FirstAsync();

            if (eventData.Equals(default(ResolvedEvent)))
            {
                await StoreCheckpoint(Position.Start.CommitPosition);
                return null;
            }

            return eventData.Deserialize<Checkpoint>()?.Position;
        }

        public Task StoreCheckpoint(ulong? checkpoint)
        {
            var @event = new Checkpoint {Position = checkpoint};

            var preparedEvent =
                new EventData(
                    Uuid.NewUuid(),
                    "$checkpoint",
                    Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(@event)
                    )
                );

            return _client.AppendToStreamAsync(
                _streamName,
                StreamState.Any,
                new List<EventData> {preparedEvent}
            );
        }

        class Checkpoint
        {
            public ulong? Position { get; set; }
        }
    }
}
