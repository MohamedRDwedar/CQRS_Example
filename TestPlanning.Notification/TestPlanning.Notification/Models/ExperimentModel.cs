using Avro;
using Avro.Specific;

namespace TestPlanning.Experiment.Models
{
	public class ExperimentModel : ISpecificRecord
	{
		public static Schema _SCHEMA = Schema.Parse("{\"type\":\"record\",\"name\":\"ExperimentModel\",\"namespace\":\"TestPlanning.Experiment.Models\"" +
				",\"fields\":[{\"name\":\"Id\",\"type\":\"long\"},{\"name\":\"Name\",\"type\":\"string\"},{\"name\":\"" +
				"MethodId\",\"type\":\"long\"},{\"name\":\"TimeStamp\",\"type\":\"long\"}]}");

		public virtual Schema Schema
		{
			get
			{
				return _SCHEMA;
			}
		}

		public long Id { get; set; }

		public string Name { get; set; }

		public long MethodId { get; set; }

		public long TimeStamp { get; set; }

		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
				case 0: return this.Id;
				case 1: return this.Name;
				case 2: return this.MethodId;
				case 3: return this.TimeStamp;
				default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}

		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
				case 0: this.Id = (long)fieldValue; break;
				case 1: this.Name = (string)fieldValue; break;
				case 2: this.MethodId = (long)fieldValue; break;
				case 3: this.TimeStamp = (long)fieldValue; break;
				default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
