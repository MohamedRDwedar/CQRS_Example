using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestPlanning.Experiment.Models;
using TestPlanning.Method.Models;
using TestPlanning.Notification.Hubs;

namespace TestPlanning.Notification.Services
{
    public class ExperimentHandlersService
    { 
        ConsumerConfig consumerConfig;
        SchemaRegistryConfig schemaRegistryConfig;

        private readonly ExperimentHub _experimentHub;
        public ExperimentHandlersService(ExperimentHub experimentHub)
        {
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
                RequestTimeoutMs = 5000,
                MaxCachedSchemas = 10
            };

            _experimentHub = experimentHub;
        }
 
        public void ExperimentCreatedHandler()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
                using (var consumerBuilder =
                    new ConsumerBuilder<KeyModel, ExperimentModel>(consumerConfig)
                        .SetKeyDeserializer(new AvroDeserializer<KeyModel>(schemaRegistry).AsSyncOverAsync())
                        .SetValueDeserializer(new AvroDeserializer<ExperimentModel>(schemaRegistry).AsSyncOverAsync())
                        .Build())
                {
                    consumerBuilder.Subscribe("ExperimentCreatedEvent");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Value != null)
                                {
                                    await _experimentHub.ExperimentCreatedMessage(consumeResult.Value);
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

        public void ExperimentUpadatedHandler()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
                using (var consumerBuilder =
                    new ConsumerBuilder<KeyModel, ExperimentModel>(consumerConfig)
                        .SetKeyDeserializer(new AvroDeserializer<KeyModel>(schemaRegistry).AsSyncOverAsync())
                        .SetValueDeserializer(new AvroDeserializer<ExperimentModel>(schemaRegistry).AsSyncOverAsync())
                        .Build())
                {
                    consumerBuilder.Subscribe("ExperimentUpadatedEvent");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Value != null)
                                {
                                    await _experimentHub.ExperimentUpadatedMessage(consumeResult.Value);
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

        public void ExperimentDeletedHandler()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
                using (var consumerBuilder =
                    new ConsumerBuilder<KeyModel, long>(consumerConfig)
                        .SetKeyDeserializer(new AvroDeserializer<KeyModel>(schemaRegistry).AsSyncOverAsync())
                        .SetValueDeserializer(new AvroDeserializer<long>(schemaRegistry).AsSyncOverAsync())
                        .Build())
                {
                    consumerBuilder.Subscribe("ExperimentDeletedEvent");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Value != 0)
                                {
                                    await _experimentHub.ExperimentDeletedMessage(consumeResult.Value);
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