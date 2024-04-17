using Confluent.Kafka;
using System;
using System.Threading;
public class KafkaConsumer : IDisposable
{
    private readonly ConsumerConfig config;
    private readonly IConsumer<string, string> consumer;
    private readonly CancellationTokenSource cancellationTokenSource;

    public KafkaConsumer(string bootstrapServers, string groupId, string topic)
    {
        // Configure the consumer
        config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);

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
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Consume a message from the topic
                    var consumeResult = consumer.Consume(cancellationToken);
                    Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' from topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}");
                }
                catch (OperationCanceledException)
                {
                    // Expected exception when cancelling the consumer
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                }
            }
        });
    }

    public void Dispose()
    {
        // Dispose the consumer and clean up resources
        consumer.Close();
        consumer.Dispose();
        cancellationTokenSource.Dispose();
    }
}