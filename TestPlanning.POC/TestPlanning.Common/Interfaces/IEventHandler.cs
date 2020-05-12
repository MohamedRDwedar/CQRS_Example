using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace TestPlanning.Common.Interfaces
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task<Result> HandleAsync(TEvent @event);
    }
}

