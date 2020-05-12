using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestPlanning.Method.Models;
using TestPlanning.Notification.Hubs;

namespace TestPlanning.Notification.Services
{
    public class MethodHandlersService
    {
        ConsumerConfig consumerConfig;
        SchemaRegistryConfig schemaRegistryConfig;

        private readonly MethodHub _methodHub;

        public MethodHandlersService(MethodHub methodHub)
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
            _methodHub = methodHub;
        }


        public void MethodCreatedHandler()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
                using (var consumerBuilder =
                    new ConsumerBuilder<KeyModel, MethodModel>(consumerConfig)
                        .SetKeyDeserializer(new AvroDeserializer<KeyModel>(schemaRegistry).AsSyncOverAsync())
                        .SetValueDeserializer(new AvroDeserializer<MethodModel>(schemaRegistry).AsSyncOverAsync())
                        .Build())
                {
                    consumerBuilder.Subscribe("MethodCreatedEvent");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Value != null)
                                {
                                    await _methodHub.MethodCreatedMessage(consumeResult.Value);
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

        public void MethodUpadatedHandler()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
                using (var consumerBuilder =
                    new ConsumerBuilder<KeyModel, MethodModel>(consumerConfig)
                        .SetKeyDeserializer(new AvroDeserializer<KeyModel>(schemaRegistry).AsSyncOverAsync())
                        .SetValueDeserializer(new AvroDeserializer<MethodModel>(schemaRegistry).AsSyncOverAsync())
                        .Build())
                {
                    consumerBuilder.Subscribe("MethodUpadatedEvent");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Value != null)
                                {
                                    await _methodHub.MethodUpadatedMessage(consumeResult.Value);
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

        public void MethodDeletedHandler()
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
                    consumerBuilder.Subscribe("MethodDeletedEvent");
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                                if (consumeResult != null && consumeResult.Value != 0)
                                {
                                    await _methodHub.MethodDeletedMessage(consumeResult.Value);
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
