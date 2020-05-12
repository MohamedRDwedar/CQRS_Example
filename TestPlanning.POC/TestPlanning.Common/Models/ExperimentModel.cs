using Newtonsoft.Json;
using System;

namespace TestPlanning.Common.Models
{
	public class ExperimentModel //: ISpecificRecord
	{
		//public static Schema _SCHEMA = Schema.Parse("{\"type\":\"record\",\"name\":\"ExperimentModel\",\"namespace\":\"TestPlanning.Common.Models\"" +
		//		",\"fields\":[{\"name\":\"Id\",\"type\":\"long\"},{\"name\":\"Name\",\"type\":\"string\"},{\"name\":\"" +
		//		"MethodId\",\"type\":\"long\"},{\"name\":\"TimeStamp\",\"type\":\"long\"}]}");

		//public virtual Schema Schema
		//{
		//	get
		//	{
		//		return _SCHEMA;
		//	}
		//}

		public ExperimentModel()
		{
			TimeStamp = TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
		}

		[JsonRequired]
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("methodId")]
		public long MethodId { get; set; }

		[JsonProperty("timeStamp")]
		public long TimeStamp { get; set; }

		//public virtual object Get(int fieldPos)
		//{
		//	switch (fieldPos)
		//	{
		//		case 0: return this.Id;
		//		case 1: return this.Name;
		//		case 2: return this.MethodId;
		//		case 3: return this.TimeStamp;
		//		default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
		//	};
		//}

		//public virtual void Put(int fieldPos, object fieldValue)
		//{
		//	switch (fieldPos)
		//	{
		//		case 0: this.Id = (long)fieldValue; break;
		//		case 1: this.Name = (string)fieldValue; break;
		//		case 2: this.MethodId = (long)fieldValue; break;
		//		case 3: this.TimeStamp = (long)fieldValue; break;
		//		default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
		//	};
		//}
	}
}
