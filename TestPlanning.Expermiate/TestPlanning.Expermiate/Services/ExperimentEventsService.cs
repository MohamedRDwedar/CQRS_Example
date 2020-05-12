using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CSharpFunctionalExtensions;
using System;
using System.Threading.Tasks;
using TestPlanning.Common.Models;

namespace TestPlanning.Experiment.Services
{
    public class ExperimentEventsService
    {
        ProducerConfig producerConfig;
        SchemaRegistryConfig schemaRegistryConfig;
        JsonSerializerConfig jsonSerializerConfig;

        public ExperimentEventsService()
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

        public Result ExperimentCreatedEvent(ExperimentModel value)
        {
            var task = Task.Run(async () =>
            {
                using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
                using var producerBuilder = new ProducerBuilder<Null, ExperimentModel>(producerConfig)
                                                .SetValueSerializer(new JsonSerializer<ExperimentModel>(schemaRegistry, jsonSerializerConfig))
                                                .Build();
                try
                {
                    var currentTicks = DateTime.Now.Ticks.ToString();
                    var producer = await producerBuilder.ProduceAsync("ExperimentCreatedEvent", new Message<Null, ExperimentModel> { Value = value });
                }
                catch (ProduceException<Null, ExperimentModel> produceException)
                {

                }
            });
            return Result.Ok();
        }

        public async Task<Result> ExperimentUpadatedEvent(ExperimentModel value)
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producerBuilder = new ProducerBuilder<Null, ExperimentModel>(producerConfig)
                                            .SetValueSerializer(new JsonSerializer<ExperimentModel>(schemaRegistry, jsonSerializerConfig))
                                            .Build();
            {
                try
                {
                    var currentTicks = DateTime.Now.Ticks.ToString();
                    var producer = await producerBuilder.ProduceAsync("ExperimentUpadatedEvent", new Message<Null, ExperimentModel> { Value = value });
                }
                catch (ProduceException<Null, ExperimentModel> produceException)
                {
                    return Result.Failure(produceException.Message);
                }
            }
            return Result.Ok();
        }

        public async Task<Result> ExperimentDeletedEvent(ExperimentModel value)
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producerBuilder = new ProducerBuilder<Null, ExperimentModel>(producerConfig)
                                            .SetValueSerializer(new JsonSerializer<ExperimentModel>(schemaRegistry, jsonSerializerConfig))
                                            .Build();
            {
                try
                {
                    var currentTicks = DateTime.Now.Ticks.ToString();
                    var producer = await producerBuilder.ProduceAsync("ExperimentDeletedEvent", new Message<Null, ExperimentModel> { Value = value });
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