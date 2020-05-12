using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace TestPlanning.Common.Interfaces
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<Result> HandleAsync(TCommand command);
    }
}
