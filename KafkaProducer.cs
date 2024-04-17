using System;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

public class KafkaProducer
{
    private readonly ProducerConfig _config;
    private readonly ILogger<KafkaProducer> _logger;

    // Constructor to initialize the KafkaProducer with bootstrap servers and logger
    public KafkaProducer(string bootstrapServers, ILogger<KafkaProducer> logger)
    {
        // Configure the producer with the provided bootstrap servers
        _config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };
        _logger = logger; // Initialize the logger
    }

    // Method to produce a message to the specified topic
    public void Produce(string topic, string message)
    {
        // Log a message indicating that a message is being produced
        _logger.LogInformation($"{DateTime.Now}: Producing message");

        // Create a new producer instance
        using var producer = new ProducerBuilder<Null, string>(_config).Build();

        // Produce the message to the specified topic
        producer.Produce(topic, new Message<Null, string> { Value = message },
            (deliveryReport) =>
            {
                // Log if there was an error delivering the message
                if (deliveryReport.Error.IsError)
                {
                    _logger.LogError($"{DateTime.Now}: Message delivery failed: {deliveryReport.Error.Reason}");
                }
                else
                {
                    // Log if the message was successfully delivered
                    _logger.LogInformation($"{DateTime.Now}: Message delivered to {deliveryReport.Topic} message is {message}");
                }
            });

        // Flush the producer to ensure all messages are sent before exiting
        producer.Flush(TimeSpan.FromSeconds(5));
    }
}
