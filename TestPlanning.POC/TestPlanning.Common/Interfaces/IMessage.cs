using FluentValidation.Results;
using Newtonsoft.Json;

namespace TestPlanning.Common.Models
{
    public interface IMessage
    {
        string MessageId { get; set; }

        long TimeStamp { get;  set; }

        ValidationResult ValidationResult { get; set; }

        public void SetValidationResult(ValidationResult validationResult);
    }
}
