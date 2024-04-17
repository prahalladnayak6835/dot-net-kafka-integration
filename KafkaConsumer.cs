using Confluent.Kafka;
using Serilog;
using System;
using System.Threading;

public class KafkaConsumer : IDisposable
{
    private readonly ConsumerConfig config;
    private readonly IConsumer<string, string> consumer;
    private readonly CancellationTokenSource cancellationTokenSource;

    // Constructor to initialize the Kafka consumer
    public KafkaConsumer(string bootstrapServers, string groupId, string topic)
    {
        // Configure the consumer
        config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        // Create a Kafka consumer
        consumer = new ConsumerBuilder<string, string>(config).Build();
        // Subscribe to the specified topic
        consumer.Subscribe(topic);

        // Initialize a CancellationTokenSource for cancellation handling
        cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Handle Ctrl+C signal to gracefully stop the consumer
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cancellationTokenSource.Cancel();
        };

        // Start a background thread to consume messages
        ThreadPool.QueueUserWorkItem(_ =>
        {
            // Continuously consume messages until cancellation is requested
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Consume a message from the topic
                    var consumeResult = consumer.Consume(cancellationToken);
                    Log.Information("Consumed message '{Message}' from topic {Topic}, partition {Partition}, offset {Offset}", consumeResult.Message.Value, consumeResult.Topic, consumeResult.Partition, consumeResult.Offset);
                }
                catch (OperationCanceledException)
                {
                    // Expected exception when cancelling the consumer
                }
                catch (ConsumeException e)
                {
                    // Handle Kafka consumer errors
                    Log.Error("Error occurred: {ErrorReason}", e.Error.Reason);
                }
                catch (Exception ex)
                {
                    // Handle other unexpected exceptions
                    Log.Error("Unexpected error occurred: {ErrorMessage}", ex.Message);
                }
            }
        });
    }

    // Method to dispose of resources when the consumer is no longer needed
    public void Dispose()
    {
        // Dispose the consumer and clean up resources
        consumer.Close();
        consumer.Dispose();
        cancellationTokenSource.Dispose();
    }
}