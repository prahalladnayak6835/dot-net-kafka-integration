using Confluent.Kafka;
using Serilog;
using System;
using System.Threading.Tasks;

public class KafkaProducer : IDisposable
{
    private readonly ProducerConfig config;
    private readonly IProducer<string, string> producer;


    // Constructor to initialize the Kafka producer
    public KafkaProducer(string bootstrapServers)
    {
        // Configure the producer
        config = new ProducerConfig { BootstrapServers = bootstrapServers };
        producer = new ProducerBuilder<string, string>(config).Build();
    }

    // Method to produce a message to the specified topic
    public async Task Produce(string topic, string message)
    {
        try
        {
            // Produce a message to the topic
            var result = await producer.ProduceAsync(topic, new Message<string, string> { Key = null, Value = message });
            Log.Information("Produced message '{Message}' to topic {Topic}, partition {Partition}, offset {Offset}", message, result.Topic, result.Partition, result.Offset);
        }
        catch (ProduceException<string, string> e)
        {
            // Handle produce exceptions
            Log.Error("Failed to produce message: {ErrorMessage} [{ErrorCode}]", e.Message, e.Error.Code);
        }
        catch (Exception ex)
        {
            // Handle other unexpected exceptions
            Log.Error("Unexpected error occurred: {ErrorMessage}", ex.Message);
        }
    }

    // Method to dispose of resources when the producer is no longer needed
    public void Dispose()
    {
        // Dispose the producer and clean up resources
        producer.Dispose();
    }
}