using System;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

public class KafkaConsumer
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<KafkaConsumer> _logger;

    // Constructor to initialize the KafkaConsumer with bootstrap servers, group id, and logger
    public KafkaConsumer(string bootstrapServers, string groupId, ILogger<KafkaConsumer> logger)
    {
        // Configure the consumer with the provided bootstrap servers, group id, and auto offset reset policy
        _config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest // Start consuming from the earliest offset
        };
        _logger = logger; // Initialize the logger
    }

    // Method to start consuming messages from the specified topic
    [Obsolete]
    public void Consume(string topic)
    {
        // Log a message indicating that message consumption is starting
        _logger.LogInformation($"{DateTime.Now}: Starting consuming messages");

        // Create a new consumer instance
        using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();

        // Subscribe to the specified topic
        consumer.Subscribe(topic);

        // Continuously consume messages from the topic
        while (true)
        {
            try
            {
                // Consume a message from the topic
                var message = consumer.Consume();

                // Log the received message
                _logger.LogInformation($"{DateTime.Now}: Received message: {message.Value}");
            }
            catch (ConsumeException e)
            {
                // Log an error if there was an error consuming the message
                _logger.LogError(e, $"{DateTime.Now}: Error occurred consuming message: {e.Error.Reason}");
            }
            catch (Exception ex)
            {
                // Log a general error if an exception occurred
                _logger.LogError(ex, $"{DateTime.Now}: General error occurred: {ex.Message}");
            }
        }
    }
}
