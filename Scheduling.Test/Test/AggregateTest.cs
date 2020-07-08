using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scheduling.EventSourcing;
using Scheduling.Infrastructure.Commands;
using Xunit;

namespace Scheduling.Test.Test
{
    public abstract class AggregateTest<TAggregate, TRepository> where TAggregate : AggregateRoot
    {
        private Dispatcher _dispatcher;

        private TRepository _repository;

        private AggregateRoot _aggregate;

        private Exception _exception;

        protected AggregateTest()
        {
            _aggregate = (AggregateRoot) Activator.CreateInstance(typeof(TAggregate));
            _repository = (TRepository) Activator.CreateInstance(typeof(TRepository), new FakeAggregateStore(_aggregate));
        }

        protected void RegisterHandlers<TCommandHandler>()
            where TCommandHandler : CommandHandler
        {

            var commandHandlerMap = new CommandHandlerMap((CommandHandler) Activator.CreateInstance(typeof(TCommandHandler), _repository));
            _dispatcher = new Dispatcher(commandHandlerMap);
        }

        protected void Given(params object[] events)
        {
            _exception = null;
            _aggregate.Load(events);
        }

        protected async Task When(object command)
        {
            try
            {
                _aggregate.ClearChanges();
                await _dispatcher.Dispatch(command, new CommandMetadata(new CorrelationId(Guid.NewGuid()), new CausationId(Guid.NewGuid())));
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        protected void Then(Action<List<object>> events)
        {
            if (_exception != null)
                throw _exception;

            events(_aggregate.GetChanges().ToList());
        }

        protected void Then<TException>() where TException : Exception
        {
           Assert.Equal(typeof(TException), _exception.GetType());
        }
    }
}
