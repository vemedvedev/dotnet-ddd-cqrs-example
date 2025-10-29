using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Shared.Notifications.Implementations;

public class NotificationPublisher : INotificationPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken ct = default)
        where TNotification : INotification
    {
        var eventType = notification.GetType();
        var handlerIfc = typeof(INotificationHandler<>).MakeGenericType(eventType);

        var handlers = (IList<object>)_serviceProvider.GetServices(handlerIfc);
        if (handlers is { Count: 0 }) return;

        foreach (var handler in handlers)
        {
            await ((dynamic)handler).HandleAsync((dynamic)notification, ct);
        }
    }
}
