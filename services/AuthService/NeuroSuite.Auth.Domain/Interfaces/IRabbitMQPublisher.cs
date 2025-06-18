namespace NeuroSuite.Auth.Domain.Interfaces
{
    public interface IRabbitMQPublisher
    {
        void PublishUserCreated(object message);
    }

}
