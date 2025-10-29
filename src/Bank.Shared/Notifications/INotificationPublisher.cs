namespace Bank.Shared.Notifications;

public interface INotificationPublisher
{
    public ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken ct = default)
        where TNotification : INotification;
}
