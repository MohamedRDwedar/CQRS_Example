using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Common.Wrappers;

namespace TestPlanning.Method.Events
{
    public class MethodEditedEvent : BaseMessage, IEvent
    {
        public string Name { get; private set; }

        public MethodEditedEvent(long id, string name)
        {
            MessageId = id.ToString();
            Name = name;
        }

        public MethodEditedEvent()
        {

        }
    }

    public class MethodEditedEventHandler : IEventHandler<MethodEditedEvent>
    {
        private readonly ProducerWrapper _producerWrapper;
        public MethodEditedEventHandler(ProducerWrapper producerWrapper)
        {
            _producerWrapper = producerWrapper;
        }

        public async Task<Result> HandleAsync(MethodEditedEvent @event)
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
