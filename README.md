# Kafka Producer-Consumer Example

This is a simple example of using Apache Kafka with C# to demonstrate message production and consumption.

## Dependencies

- [.NET Core SDK](https://dotnet.microsoft.com/download) (or .NET Framework)
- [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka/) package

## Setup

1. Clone or download this repository.
2. Install the required dependencies mentioned above.
3. Configure your Kafka broker(s) and update the `bootstrapServers` variable in the `Program.cs` file accordingly.

## Usage

1. Compile and run the program using the .NET CLI or your preferred IDE.
2. The program will produce a message to the specified Kafka topic and then consume messages from the same topic.

## Code Explanation

- `KafkaProducer.cs`: Contains a class for producing messages to a Kafka topic.
- `KafkaConsumer.cs`: Contains a class for consuming messages from a Kafka topic.
- `Program.cs`: Main entry point of the application where a message is produced and then consumed.

## Configuration

- Update the `bootstrapServers` variable in `Program.cs` with your Kafka broker's address and port.
- Adjust the `topic` variable in `Program.cs` to match the Kafka topic you want to produce and consume messages from.

## Logging

- The program utilizes Microsoft.Extensions.Logging for logging.
- Logs are output to the console with timestamps.

## Notes

- This example uses the `Obsolete` attribute, which may be deprecated in future versions of .NET.
- Ensure that your Kafka broker is running and accessible from the application.


## Run Kafka:

- Start Zookeeper (if not already running): `bin\windows\zookeeper-server-start.bat config\zookeeper.properties`
- Start Kafka server: `bin\windows\kafka-server-start.bat config\server.properties`
- Create a Kafka topic: `bin\windows\kafka-topics.bat --create --topic user-topic --bootstrap-server localhost:9092`
- Start a Kafka producer: `bin\windows\kafka-console-producer.bat --topic user-topic --bootstrap-server localhost:9092`
- Start a Kafka consumer: `bin\windows\kafka-console-consumer.bat --topic user-topic --from-beginning --bootstrap-server localhost:9092`