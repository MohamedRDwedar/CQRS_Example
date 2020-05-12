using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CSharpFunctionalExtensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestPlanning.Experiment.Context;
using TestPlanning.Common.Models;

namespace TestPlanning.Experiment.Services
{
    public class ExperimentHandlersService
    {
        ConsumerConfig consumerConfig;
        SchemaRegistryConfig schemaRegistryConfig;
        JsonSerializerConfig jsonSerializerConfig;

        private readonly ExperimentEventsService _experimentEventsService;

        public ExperimentHandlersService(ExperimentEventsService experimentEventsService)
        {
            consumerConfig = new ConsumerConfig
            {
                GroupId = "experiment-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            //schemaRegistryConfig = new SchemaRegistryConfig
            //{
            //    Url = "http://localhost:8081",
            //    // optional schema registry client properties:
            //    RequestTimeoutMs = 5000,
            //    MaxCachedSchemas = 10
            //};

            //jsonSerializerConfig = new JsonSerializerConfig
            //{
            //    BufferBytes = 100,
            //    AutoRegisterSchemas = true,
            //};

            _experimentEventsService = experimentEventsService;
        }

        public Result<ExperimentModel> GetExperiment(long id)
        {
            using (var experimentContext = new ExperimentContext())
            {
                var Experiment = experimentContext.Experiments.FirstOrDefault(c => c.Id == id);
                if (Experiment != null)
                {
                    return Result.Ok(Experiment);
                }
            }
            return Result.Failure<ExperimentModel>(null);
        }

        public Result CreateExperimentHandler()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                using var consumerBuilder = new ConsumerBuilder<Null, ExperimentModel>(consumerConfig)
                                               .SetValueDeserializer(new JsonDeserializer<ExperimentModel>().AsSyncOverAsync())
                                               .Build();
                consumerBuilder.Subscribe("CreateExperimentCommand");
                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                            if (consumeResult != null && consumeResult.Message != null)
                            {
                                using var experimentContext = new ExperimentContext();
                                try
                                {
                                    var experiment = consumeResult.Message.Value;
                                    experimentContext.Experiments.Add(experiment);
                                    experimentContext.SaveChanges();
                                    _experimentEventsService.ExperimentCreatedEvent(experiment);
                                }
                                catch (Exception exception)
                                {

                                }
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

        public void UpadateExperimentHandler()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using var consumerBuilder = new ConsumerBuilder<Null, ExperimentModel>(consumerConfig)
                                                 .SetValueDeserializer(new JsonDeserializer<ExperimentModel>().AsSyncOverAsync())
                                                 .Build();
                {
                    consumerBuilder.Subscribe("UpadateExperimentCommand");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Message.Value != null)
                                {
                                    var Experiment = GetExperiment(consumeResult.Message.Value.Id);
                                    if (Experiment.IsSuccess)
                                    {
                                        using (var experimentContext = new ExperimentContext())
                                        {
                                            var existExperiment = Experiment.Value;
                                            existExperiment.Name = consumeResult.Message.Value.Name;
                                            existExperiment.TimeStamp = consumeResult.Message.Value.TimeStamp;
                                            experimentContext.Experiments.Update(existExperiment);
                                            experimentContext.SaveChanges();

                                            await _experimentEventsService.ExperimentUpadatedEvent(existExperiment);
                                        }
                                    }
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
        }

        public void DeleteExperimentHandler()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using var consumerBuilder = new ConsumerBuilder<Null, ExperimentModel>(consumerConfig)
                                               .SetValueDeserializer(new JsonDeserializer<ExperimentModel>().AsSyncOverAsync())
                                               .Build();
                {
                    consumerBuilder.Subscribe("DeleteExperimentCommand");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Message != null)
                                {
                                    var Experiment = GetExperiment(consumeResult.Message.Value.Id);
                                    if (Experiment.IsSuccess)
                                    {
                                        using (var experimentContext = new ExperimentContext())
                                        {
                                            experimentContext.Experiments.Remove(Experiment.Value);
                                            experimentContext.SaveChanges();
                                            await _experimentEventsService.ExperimentDeletedEvent(Experiment.Value);
                                        }
                                    }
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
        }
    }
}