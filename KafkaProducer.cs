using Confluent.Kafka;
using System;
using System.Threading;

public class KafkaProducer : IDisposable
{
    private readonly ProducerConfig config;
    private readonly IProducer<string, string> producer;

    public KafkaProducer(string bootstrapServers)
    {
        // Configure the producer
        config = new ProducerConfig { BootstrapServers = bootstrapServers };
        producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async void Produce(string topic, string message)
    {
        try
        {
            // Produce a message to the topic
            var result = await producer.ProduceAsync(topic, new Message<string, string> { Key = null, Value = message });
            Console.WriteLine($"Produced message '{message}' to topic {result.Topic}, partition {result.Partition}, offset {result.Offset}");
        }
        catch (ProduceException<string, string> e)
        {
            Console.WriteLine($"Failed to produce message: {e.Message} [{e.Error.Code}]");
        }
    }

    public void Dispose()
    {
        // Dispose the producer
        producer.Dispose();
    }
}