using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Common.Wrappers;

namespace TestPlanning.Method.CommandHandlers
{
    public class AddMethodCommand : BaseMessage, ICommand // : Command
    {
        public AddMethodCommand()
        {

        }

        public AddMethodCommand(string name)
        {
            Name = name;
        }

        [JsonProperty]
        public string Name { get; set; }
    }

    public class AddMethodCommandHandler : ICommandHandler<AddMethodCommand>
    {
        private readonly ProducerWrapper _producerWrapper;

        public AddMethodCommandHandler(ProducerWrapper producerWrapper)
        {
            _producerWrapper = producerWrapper;
        }

        public async Task<Result> HandleAsync(AddMethodCommand command)
        {
            if (command.ValidationResult.IsValid)
            {
                var result = await _producerWrapper.ProduceMessage(command.GetType().Name, command);
                return result;
            }
            else
            {
                return Result.Failure(command.ValidationResult.Errors.ToString());
            }
        }
    }
}
