namespace Bank.Shared.Notifications;

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    public ValueTask HandleAsync(TNotification notification, CancellationToken ct = default);
}
