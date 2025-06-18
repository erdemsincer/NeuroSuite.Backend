using Microsoft.Extensions.DependencyInjection;
using NeuroSuite.BuildingBlocks.Contracts.Auth;
using NeuroSuite.User.Domain.Entities;
using NeuroSuite.User.Domain.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NeuroSuite.User.Infrastructure.Messaging;

public class RabbitMQConsumer : IRabbitMQConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMQConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void Start()
    {
        var factory = new ConnectionFactory { HostName = "rabbitmq" };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "user-created-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var @event = JsonConvert.DeserializeObject<UserCreatedEvent>(message);

            if (@event is null) return;

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IUserProfileRepository>();

            var user = new UserProfile
            {
                Id = @event.Id,
                Email = @event.Email,
                FullName = @event.FullName,
                Role = @event.Role
            };

            await repo.AddAsync(user);
        };

        channel.BasicConsume(queue: "user-created-queue", autoAck: true, consumer: consumer);

        // Uygulama kapanmasın diye bekletiyoruz
        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}
