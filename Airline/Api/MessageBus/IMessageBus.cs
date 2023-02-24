using Api.Models;

namespace Api.MessageBus;

public interface IMessageBus
{
    void Publish(NotificationMessage notificationMessage);
}