using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CSharpFunctionalExtensions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Threading.Tasks;
using TestPlanning.Common.Models;

namespace TestPlanning.Common.Wrappers
{
    // https://github.com/confluentinc/schema-registry/issues/1126
    // https://medium.com/@swetavkamal/aws-msk-with-schema-registry-55edc016cd5e
    // https://stackoverflow.com/questions/55408193/amazon-managed-streaming-for-kafka-msk-features-and-performance
    // https://lenses.io/blog/2019/12/lenses-and-aws-msk-collapsing-delivery-timeframes-open-monitoring/
    // 
    public class ProducerWrapper
    {
        ProducerConfig producerConfig;
        SchemaRegistryConfig schemaRegistryConfig;
        JsonSerializerConfig jsonSerializerConfig;

        public ProducerWrapper()
        {
            producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = "http://localhost:8081",
            };

            jsonSerializerConfig = new JsonSerializerConfig
            {
                SubjectNameStrategy = SubjectNameStrategy.TopicRecord,
                AutoRegisterSchemas = true,
                
            };
        }

        public async Task<Result> ProduceMessage<tModel>(string topicName, tModel model) where tModel : BaseMessage, new()
        {
            try
            {
                using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
                using var producerBuilder = new ProducerBuilder<string, tModel>(producerConfig)
                                                .SetValueSerializer(new JsonSerializer<tModel>(schemaRegistry, jsonSerializerConfig))
                                                .Build();
                try
                {
                    var producer = await producerBuilder.ProduceAsync(topicName, new Message<string, tModel>
                    {
                        Key = model.MessageId,
                        Timestamp = new Timestamp(model.TimeStamp, TimestampType.CreateTime),
                        Value = model
                    });
                    var result = Result.Success("HELLO");
                    return result;
                }
                catch (ProduceException<Null, tModel> produceException)
                {
                    return Result.Failure(produceException.InnerException?.Message);
                }
            }
            catch (Exception exception)
            {
                return Result.Failure(exception.InnerException?.Message);
            }
        }
    }
}
