using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Common.Wrappers;

namespace TestPlanning.Method.CommandHandlers
{
    public class DeleteMethodCommand : BaseMessage, ICommand
    {
        public long Id { get; set; }

        public DeleteMethodCommand()
        {

        }

        public DeleteMethodCommand(long id)
        {
            MessageId = id.ToString();
            Id = id;
        }
    }

    public class DeleteMethodCommandHandler : ICommandHandler<DeleteMethodCommand>
    {
        private readonly ProducerWrapper _producerWrapper;
        public DeleteMethodCommandHandler(ProducerWrapper producerWrapper)
        {
            _producerWrapper = producerWrapper;
        }

        public async Task<Result> HandleAsync(DeleteMethodCommand command)
        {
            if (command.ValidationResult.IsValid)
            {
                return await _producerWrapper.ProduceMessage(command.GetType().Name, command);
            }
            else
            {
                return Result.Failure(command.ValidationResult.Errors.ToString());
            }
        }
    }
}

