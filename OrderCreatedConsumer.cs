using ConsumerAPI.Conection;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConsumerAPI;

public class OrderCreatedConsumer(RabbitMqConnection connection) : BackgroundService
{
    private const string QueueName = "order-created-queue";

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false, cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, args) =>
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());

            Console.WriteLine($"Mensagem recebida: {message}");

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: true,
            consumerTag: string.Empty,
            noLocal: false,
            exclusive: false,
            arguments: null,
            consumer: consumer,
            cancellationToken: cancellationToken
        );
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
}