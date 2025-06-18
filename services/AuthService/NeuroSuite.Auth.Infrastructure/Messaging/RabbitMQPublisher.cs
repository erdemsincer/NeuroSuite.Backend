using NeuroSuite.Auth.Domain.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace NeuroSuite.Auth.Infrastructure.Messaging;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    public void PublishUserCreated(object message)
    {
        var factory = new ConnectionFactory { HostName = "rabbitmq" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "user-created-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: "user-created-queue", basicProperties: null, body: body);
    }
}
