using Avro;
using Avro.Specific;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestPlanning.Method.Models
{

    // https://github.com/confluentinc/confluent-kafka-dotnet/blob/master/examples/AvroSpecific/Program.cs
    // https://www.confluent.io/blog/decoupling-systems-with-apache-kafka-schema-registry-and-avro/
    public class MethodModel : ISpecificRecord
    {
        public static Schema _SCHEMA = Schema.Parse("{\"type\":\"record\",\"name\":\"MethodModel\",\"namespace\":\"TestPlanning.Method.Models\",\"fields" +
                     "\":[{\"name\":\"Id\",\"type\":\"long\"},{\"name\":\"Name\",\"type\":\"string\"}, {\"name\":\"TimeStamp\",\"type\":\"long\"}]}");

        public virtual Schema Schema
        {
            get
            {
                return MethodModel._SCHEMA;
            }
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public long TimeStamp { get; set; }

        public virtual object Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return this.Id;
                case 1: return this.Name;
                case 2: return this.TimeStamp;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
            };
        }
        public virtual void Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: this.Id = (long)fieldValue; break;
                case 1: this.Name = (string)fieldValue; break;
                case 2: this.TimeStamp = (long)fieldValue; break;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
            };
        }
    }
}
