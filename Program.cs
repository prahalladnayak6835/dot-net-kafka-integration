using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

class Program
{
    [Obsolete]
    static void Main(string[] args)
    {
        // Create a service collection and configure logging
        var services = new ServiceCollection()
            .AddLogging(builder =>
            {
                // Add console logging with timestamps in the output format
                builder.AddConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
                });
            })
            .BuildServiceProvider();

        // Get a logger for the Program class
        var logger = services.GetRequiredService<ILogger<Program>>();

        // Log a message indicating that the application is starting
        logger.LogInformation("Starting application");

        string bootstrapServers = "localhost:9092";
        string topic = "user-topic";

        // Pass the logger instance to KafkaProducer constructor
        var producer = new KafkaProducer(bootstrapServers, services.GetRequiredService<ILogger<KafkaProducer>>());
        try
        {
            // Produce a message to the specified topic
            producer.Produce(topic, "Hello, prahallad");
        }
        catch (Exception ex)
        {
            // Log an error if there was an error producing the message
            logger.LogError(ex, "Error producing message");
        }

        // Pass the logger instance to KafkaConsumer constructor
        var consumer = new KafkaConsumer(bootstrapServers, "my-consumer-group", services.GetRequiredService<ILogger<KafkaConsumer>>());
        try
        {
            // Consume messages from the specified topic
            consumer.Consume(topic);
        }
        catch (Exception ex)
        {
            // Log an error if there was an error consuming messages
            logger.LogError(ex, "Error consuming messages");
        }

        // Log a message indicating that the application has completed
        logger.LogInformation("Application completed");

        // Wait for user input before exiting
        Console.ReadLine();
    }
}
