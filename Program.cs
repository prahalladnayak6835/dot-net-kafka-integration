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
                // Wait for the user to press Enter
                Console.ReadLine(); 
            }

            using (var consumer = new KafkaConsumer(bootstrapServers, "test-consumer-group", topic))
            {
                // Wait for the user to press Enter
                Console.ReadLine(); 
            }
        }
    }
}
