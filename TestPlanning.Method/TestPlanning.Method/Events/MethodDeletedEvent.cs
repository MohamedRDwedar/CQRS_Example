using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Common.Wrappers;

namespace TestPlanning.Method.Events
{
    public class MethodDeletedEvent : BaseMessage, IEvent
    {
        public string Name { get; private set; }

        public MethodDeletedEvent(long id, string name)
        {
            MessageId = id.ToString();
            Name = name;
        }

        public MethodDeletedEvent()
        {

        }
    }

    public class MethodDeletedEventHandler : IEventHandler<MethodDeletedEvent>
    {
        private readonly ProducerWrapper _producerWrapper;
        public MethodDeletedEventHandler(ProducerWrapper producerWrapper)
        {
            _producerWrapper = producerWrapper;
        }

        public async Task<Result> HandleAsync(MethodDeletedEvent @event)
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
