using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using CSharpFunctionalExtensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestPlanning.Common.Models;

namespace TestPlanning.Common.Wrappers
{
    public class ConumerWrapper
    {
        ConsumerConfig consumerConfig;
        //SchemaRegistryConfig schemaRegistryConfig;
        JsonSerializerConfig jsonSerializerConfig;

        public ConumerWrapper()
        {
            consumerConfig = new ConsumerConfig
            {
                GroupId = "method-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            //schemaRegistryConfig = new SchemaRegistryConfig
            //{
            //    Url = "http://localhost:8081",
            //    // optional schema registry client properties:
            //    RequestTimeoutMs = 5000,
            //    MaxCachedSchemas = 10
            //};

            jsonSerializerConfig = new JsonSerializerConfig();
        }

        public void ConumeMessage<tModel>(string topicName, Action<tModel> action) where tModel : class, new()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Factory.StartNew(() =>
            {
                using var consumerBuilder = new ConsumerBuilder<string, tModel>(consumerConfig)
                                               .SetValueDeserializer(new JsonDeserializer<tModel>(jsonSerializerConfig).AsSyncOverAsync())
                                                .Build();
                consumerBuilder.Subscribe(topicName);
                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumerBuilder.Consume(cancellationTokenSource.Token);
                            if (consumeResult != null && consumeResult.Message != null)
                            {
                                action(consumeResult.Message.Value);
                            }
                        }
                        catch (ConsumeException consumeException)
                        {

                        }
                    }
                }
                catch (OperationCanceledException operationCanceledException)
                {
                    consumerBuilder.Close();
                }
            });
        }
    }
}
