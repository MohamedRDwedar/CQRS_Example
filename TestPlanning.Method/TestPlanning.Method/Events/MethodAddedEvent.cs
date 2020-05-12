using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Common.Wrappers;

namespace TestPlanning.Method.Events
{
    public class MethodAddedEvent: BaseMessage, IEvent
    {
        public string Name { get; private set; }

        public MethodAddedEvent(long id, string name)
        {
            MessageId = id.ToString();
            Name = name;
        }

        public MethodAddedEvent()
        {

        }
    }

    public class MethodAddedEventHandler : IEventHandler<MethodAddedEvent>
    {
        private readonly ProducerWrapper _producerWrapper;
        public MethodAddedEventHandler(ProducerWrapper producerWrapper)
        {
            _producerWrapper = producerWrapper;
        }

        public async Task<Result> HandleAsync(MethodAddedEvent @event)
        {
            if (@event.ValidationResult.IsValid)
            {
                return await _producerWrapper.ProduceMessage(@event.GetType().Name, @event);
            }
            else
            {
                return Result.Failure(@event.ValidationResult.Errors.ToString());
            }
        }
    }
}
