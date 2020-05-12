using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using TestPlanning.Common.Models;

namespace TestPlanning.Experiment.Services
{
    public class ExperimentCommandsService
    {
        ProducerConfig producerConfig;
        SchemaRegistryConfig schemaRegistryConfig;
        JsonSerializerConfig jsonSerializerConfig;

        public ExperimentCommandsService()
        {
            producerConfig = new ProducerConfig
            {
               BootstrapServers = "localhost:9092"
            };

            schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = "http://localhost:8081",
                // optional schema registry client properties:
                RequestTimeoutMs = 5000,
                MaxCachedSchemas = 10
            };

            jsonSerializerConfig = new JsonSerializerConfig
            {
                BufferBytes = 100
            };
        }

        public Result CreateExperimentCommand(ExperimentModel value)
        {
            var task = Task.Run(() =>
            {
                using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
                using var producerBuilder = new ProducerBuilder<Null, ExperimentModel>(producerConfig)
                                                .SetValueSerializer(new JsonSerializer<ExperimentModel>(schemaRegistry, jsonSerializerConfig))
                                                .Build();
                try
                {
                    var producer = producerBuilder.ProduceAsync("CreateExperimentCommand", new Message<Null, ExperimentModel> { Value = value });
                }
                catch (ProduceException<Null, ExperimentModel> produceException)
                {

                }
            });
            return Result.Ok();
        }

        public async Task<Result> UpadateExperimentCommand(long id, ExperimentModel value)
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producerBuilder = new ProducerBuilder<Null, ExperimentModel>(producerConfig)
                                            .SetValueSerializer(new JsonSerializer<ExperimentModel>(schemaRegistry, jsonSerializerConfig))
                                            .Build();
            {
                try
                {
                    value.Id = id;
                    var producer = await producerBuilder.ProduceAsync("UpadateExperimentCommand", new Message<Null, ExperimentModel> { Value = value });
                }
                catch (ProduceException<Null, ExperimentModel> produceException)
                {
                    return Result.Failure(produceException.Message);
                }
            }
            return Result.Ok();
        }

        public async Task<Result> DeleteExperimentCommand(ExperimentModel value)
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producerBuilder = new ProducerBuilder<Null, ExperimentModel>(producerConfig)
                                            .SetValueSerializer(new JsonSerializer<ExperimentModel>(schemaRegistry, jsonSerializerConfig))
                                            .Build();
            {
                try
                {
                    var producer = await producerBuilder.ProduceAsync("DeleteExperimentCommand", new Message<Null, ExperimentModel> { Value = value });
                }
                catch (ProduceException<Null, ExperimentModel> produceException)
                {
                    return Result.Failure(produceException.Message);
                }
            }
            return Result.Ok();
        }
    }
}