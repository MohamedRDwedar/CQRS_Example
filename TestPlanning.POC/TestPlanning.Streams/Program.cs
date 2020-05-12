using Confluent.Kafka;
using kafka_stream_core;
using kafka_stream_core.SerDes;
using kafka_stream_core.Stream;
using kafka_stream_core.Table;
using System;
using System.Threading;

namespace TestPlanning.Streams
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();

            //var config = new StreamConfig<AvroSerializer, AvroSerializer>
            //{
            //    ApplicationId = "test-app",
            //    BootstrapServers = "localhost:9092",
            //    AutoOffsetReset = AutoOffsetReset.Earliest,
            //    NumStreamThreads = 2
            //};

            //StreamBuilder builder = new StreamBuilder();

            //builder.Stream<long, string>("test")
            //    .FilterNot((k, v) => v.Contains("test"))
            //    .Peek((k, v) => Console.WriteLine($"Key : {k} | Value : {v}"))
            //    .To("test-output");

            //Topology t = builder.Build();
            //KafkaStream stream = new KafkaStream(t, config);

            //Console.CancelKeyPress += (o, e) => {
            //    source.Cancel();
            //    stream.Close();
            //};

            //stream.Start(source.Token);
        }
    }
}
