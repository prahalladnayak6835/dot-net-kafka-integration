using System;

namespace KafkaIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            string bootstrapServers = "localhost:9092";
            string topic = "user-topic";

            using (var producer = new KafkaProducer(bootstrapServers))
            {
                producer.Produce(topic, "Hello Kafka!");
                Console.ReadLine(); // Wait for the user to press Enter
            }

            using (var consumer = new KafkaConsumer(bootstrapServers, "test-consumer-group", topic))
            {
                Console.ReadLine(); // Wait for the user to press Enter
            }
        }
    }
}
