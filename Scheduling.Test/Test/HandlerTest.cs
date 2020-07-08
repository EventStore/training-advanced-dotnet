using System.Linq;
using System.Threading.Tasks;
using Scheduling.Domain.Infrastructure.Projections;
using Xunit;

namespace Scheduling.Test.Test
{
    public abstract class HandlerTest
    {
        protected abstract EventHandler GetHandler();

        private EventHandler _eventHandler;

        protected async Task Given(params object[] events)
        {
            _eventHandler = GetHandler();
            foreach (var @event in events)
            {
                await _eventHandler.Handle(@event.GetType(), @event);
            }
        }

        protected void Then(object expected, object actual)
        {
            Assert.Equal(expected, actual);
        }
    }
}
