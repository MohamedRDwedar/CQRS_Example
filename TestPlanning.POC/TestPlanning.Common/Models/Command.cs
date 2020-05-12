using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using TestPlanning.Common.Interfaces;

namespace TestPlanning.Common.Commands
{
    public class Command : ICommand
    {
        [JsonProperty("TimeStamp")]
        public long TimeStamp { get; private set; }

        //[JsonIgnore]
        //public ValidationResult ValidationResult { get; private set; }


        protected Command()
        {
            TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }

        //public void SetValidationResult(ValidationResult validationResult)
        //{
        //    if (validationResult == null)
        //    {
        //        ValidationResult = new ValidationResult();
        //    }
        //    else
        //    {
        //        ValidationResult = validationResult;
        //    }
        //}
    }
}
