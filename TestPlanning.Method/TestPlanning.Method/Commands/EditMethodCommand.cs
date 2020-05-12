using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Common.Wrappers;

namespace TestPlanning.Method.CommandHandlers
{
    public sealed class EditMethodCommand : BaseMessage, ICommand // : Command
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public EditMethodCommand()
        {

        }

        public EditMethodCommand(long id, string name)
        {
            MessageId = id.ToString();
            Id = id;
            Name = name;
        }
    }

    public sealed class EditMethodCommandHandler : ICommandHandler<EditMethodCommand>
    {
        private readonly ProducerWrapper _producerWrapper;

        public EditMethodCommandHandler(ProducerWrapper producerWrapper)
        {
            _producerWrapper = producerWrapper;
        }

        public async Task<Result> HandleAsync(EditMethodCommand command)
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
