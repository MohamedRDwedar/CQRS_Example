using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CSharpFunctionalExtensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestPlanning.Common.Models;
using TestPlanning.WorkFlow.Models;

namespace TestPlanning.WorkFlow.Service
{
    // very very very important https://stackoverflow.com/questions/56580069/elasticsearchsinkconnector-cant-connect-to-elastic/
    // https://github.com/App-vNext/Polly/
    // https://cpratt.co/async-tips-tricks/
    // https://stackify.com/csharp-exception-handling-best-practices/
    // https://github.com/eventflow/EventFlow/
    // https://docs.microsoft.com/en-us/archive/msdn-magazine/2016/october/cutting-edge-event-command-saga-approach-for-business-logic/
    // https://stackoverflow.com/questions/43408599/choosing-a-nosql-database-for-storing-events-in-a-cqrs-designed-application

    public class ExperimentMethodSagaService
    {
        ProducerConfig producerConfig;
        ConsumerConfig consumerConfig;
        SchemaRegistryConfig schemaRegistryConfig;
        JsonSerializerConfig jsonSerializerConfig;

        public ExperimentMethodSagaService()
        {
            producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            consumerConfig = new ConsumerConfig
            {
                GroupId = "experiment-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = "http://localhost:8081",
                // optional schema registry client properties:
                //RequestTimeoutMs = 5000,
                //MaxCachedSchemas = 10
            };

            jsonSerializerConfig = new JsonSerializerConfig
            {
                BufferBytes = 100
            };

        }
        public Result CreateExperminentMethodSaga(ExperimentMethodModel value)
        {
            var method = new MethodModel { Id = value.MethodId, Name = value.MethodName, TimeStamp = value.MethodTimeStamp };
            var experiment = new ExperimentModel { Id = value.ExperimentId, Name = value.ExperimentName, TimeStamp = value.ExperimentTimeStamp };

            MethodCreatedHandler(experiment);
            CreateMethodCommand(method);
            return Result.Ok();
        }


        public async Task<Result> UpdateExperminentMethodSaga(ExperimentMethodModel value)
        {
            var method = new MethodModel { Id = value.MethodId, Name = value.MethodName, TimeStamp = value.MethodTimeStamp };
            var experiment = new ExperimentModel { Id = value.ExperimentId, Name = value.ExperimentName, TimeStamp = value.ExperimentTimeStamp };

            await UpdateMethodCommand(method);
            return MethodUpdatedHandler(experiment);
        }

        public async Task<Result> DeleteExperminentMethodSaga(ExperimentMethodModel value)
        {
            var method = new MethodModel { Id = value.MethodId, Name = value.MethodName, TimeStamp = value.MethodTimeStamp };
            var experiment = new ExperimentModel { Id = value.ExperimentId, Name = value.ExperimentName, TimeStamp = value.ExperimentTimeStamp };
            await DeleteMethodCommand(method);
            return MethodDeletedHandler(experiment);
        }

        public Result CreateMethodCommand(MethodModel value)
        {
            var task = Task.Run(async () =>
            {
                using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
                using var producerBuilder = new ProducerBuilder<Null, MethodModel>(producerConfig)
                                                .SetValueSerializer(new JsonSerializer<MethodModel>(schemaRegistry, jsonSerializerConfig))
                                                .Build();
                try
                {
                    var producer = await producerBuilder.ProduceAsync("CreateMethodCommand", new Message<Null, MethodModel> { Value = value });
                }
                catch (ProduceException<Null, MethodModel> produceException)
                {

                }
            });
            return Result.Ok();
        }

        public Result MethodCreatedHandler(ExperimentModel value)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                using var consumerBuilder = new ConsumerBuilder<Null, MethodModel>(consumerConfig)
                                               .SetValueDeserializer(new JsonDeserializer<MethodModel>().AsSyncOverAsync())
                                                .Build();
                consumerBuilder.Subscribe("MethodCreatedEvent");
                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                            if (consumeResult != null && consumeResult.Message != null)
                            {
                                value.MethodId = consumeResult.Message.Value.Id;
                                CreateExperimentCommand(value);
                                consumerBuilder.Unsubscribe();
                            }
                        }
                        catch (ConsumeException consumeException)
                        {

                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            });
            return Result.Ok();
        }

        public Result CreateExperimentCommand(ExperimentModel value)
        {
            var task = Task.Run(async( ) =>
            {
                using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
                using var producerBuilder = new ProducerBuilder<Null, ExperimentModel>(producerConfig)
                                                .SetValueSerializer(new JsonSerializer<ExperimentModel>(schemaRegistry, jsonSerializerConfig))
                                                .Build();
                try
                {
                    var producer = await producerBuilder.ProduceAsync("CreateExperimentCommand", new Message<Null, ExperimentModel> { Value = value });
                }
                catch (ProduceException<Null, ExperimentModel> produceException)
                {

                }
            });
            return Result.Ok();
        }

        public async Task<Result> UpdateMethodCommand(MethodModel value)
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producerBuilder = new ProducerBuilder<Null, MethodModel>(producerConfig)
                                            .SetValueSerializer(new JsonSerializer<MethodModel>(schemaRegistry, jsonSerializerConfig))
                                            .Build();
            {
                try
                {
                    var producer = await producerBuilder.ProduceAsync("UpdateMethodCommand", new Message<Null, MethodModel> { Value = value });
                }
                catch (ProduceException<Null, MethodModel> produceException)
                {
                    return Result.Failure(produceException.Message);
                }
            }
            return Result.Ok();
        }

        public Result MethodUpdatedHandler(ExperimentModel value)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using var consumerBuilder = new ConsumerBuilder<Null, MethodModel>(consumerConfig)
                                                .SetValueDeserializer(new JsonDeserializer<MethodModel>().AsSyncOverAsync())
                                                 .Build();
                {
                    consumerBuilder.Subscribe("MethodUpdatedEvent");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Message != null)
                                {
                                    value.MethodId = consumeResult.Message.Value.Id;
                                    await UpdateExperimentCommand(value);
                                    consumerBuilder.Unsubscribe();
                                }
                            }
                            catch (ConsumeException consumeException)
                            {

                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            });
            return Result.Ok();
        }

        public async Task<Result> UpdateExperimentCommand(ExperimentModel value)
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producerBuilder = new ProducerBuilder<Null, ExperimentModel>(producerConfig)
                                            .SetValueSerializer(new JsonSerializer<ExperimentModel>(schemaRegistry, jsonSerializerConfig))
                                            .Build();
            {
                try
                {
                    var producer = await producerBuilder.ProduceAsync("UpdateExperimentCommand", new Message<Null, ExperimentModel> { Value = value });
                }
                catch (ProduceException<Null, ExperimentModel> produceException)
                {
                    return Result.Failure(produceException.Message);
                }
            }
            return Result.Ok();
        }

        public async Task<Result> DeleteMethodCommand(MethodModel value)
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producerBuilder = new ProducerBuilder<Null, MethodModel>(producerConfig)
                                            .SetValueSerializer(new JsonSerializer<MethodModel>(schemaRegistry, jsonSerializerConfig))
                                            .Build();
            {
                try
                {
                    var producer = await producerBuilder.ProduceAsync("DeleteMethodCommand", new Message<Null, MethodModel> { Value = value });
                }
                catch (ProduceException<Null, MethodModel> produceException)
                {
                    return Result.Failure(produceException.Message);
                }
            }
            return Result.Ok();
        }

        public Result MethodDeletedHandler(ExperimentModel value)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using var consumerBuilder = new ConsumerBuilder<Null, MethodModel>(consumerConfig)
                                               .SetValueDeserializer(new JsonDeserializer<MethodModel>().AsSyncOverAsync())
                                                .Build();
                {
                    consumerBuilder.Subscribe("MethodDeletedEvent");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Message != null)
                                {
                                    await DeleteExperimentCommand(value);
                                    consumerBuilder.Unsubscribe();
                                }
                            }
                            catch (ConsumeException consumeException)
                            {

                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            });
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
                    var currentTicks = DateTime.Now.Ticks.ToString();
                    var producer = await producerBuilder.ProduceAsync("DeleteExperimentCommand", new Message<Null, ExperimentModel> { Value = value });
                }
                catch (ProduceException<Null, long> produceException)
                {
                    return Result.Failure(produceException.Message);
                }
            }
            return Result.Ok();
        }
    }
}
