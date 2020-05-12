using System;

namespace TestPlanning.Method.Models
{
    public class KeyModel : ISpecificRecord
    {
        public static Schema _SCHEMA = Schema.Parse("{\"type\":\"record\",\"name\":\"KeyModel\",\"namespace\":\"TestPlanning.Method.Models\",\"fields" +
              "\":[{\"name\":\"UserId\",\"type\":\"long\"},{\"name\":\"TimeStamp\",\"type\":\"long\"}]}");

        public long UserId { get; set; }
        public long TimeStamp { get; set; }

        public virtual Schema Schema
        {
            get
            {
                return _SCHEMA;
            }
        }

        public static KeyModel DefultKey()
        {
            return new KeyModel { TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() };
        }

        public object Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return this.UserId;
                case 1: return this.TimeStamp;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
            };
        }

        public void Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: this.UserId = (long)fieldValue; break;
                case 1: this.TimeStamp = (long)fieldValue; break;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
            };
        }
    }
}
