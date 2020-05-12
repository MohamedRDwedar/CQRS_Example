using FluentValidation.Results;
using Newtonsoft.Json;
using System;

namespace TestPlanning.Common.Models
{
    public class BaseMessage
    {
        public  BaseMessage()
        {
            MessageId = default(long).ToString();
            TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            ValidationResult = new ValidationResult();
        }

        [JsonProperty]
        public string MessageId { get; set; }

        [JsonProperty]
        public long TimeStamp { get; set; }

        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        public void SetValidationResult(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}
