using CSharpFunctionalExtensions;

namespace TestPlanning.Common.Interfaces
{
    public interface ICommandSubscriber<TCommand> where TCommand : ICommand
    {
        void Handle();
    }
}
