using DMHexagonal.Catalog.Core.Base.Abstractions;

namespace DMHexagonal.Catalog.Infra.Tools;

public class DomainEventDispatcher : IDispatcher
{
    private readonly Lock _lock = new();
    private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new();
    private readonly List<IMessage> _events = new();

    public void RegisterHandler<T>(Action<T> handler)
        where T : IMessage
    {
        lock (_lock)
        {
            if (!_routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Action<IMessage>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add(message => handler((T)message));
        }
    }

    public void Publish(IEnumerable<IMessage> events)
    {
        lock (_lock)
        {
            _events.AddRange(events);
        }
    }

    public void Run()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                lockedHandle();

                await Task.Delay(10);
            }
        });
    }

    private void lockedHandle()
    {
        lock (_lock)
        {
            try
            {
                handleAllEvents();
            }
            catch
            {
                throw;
            }
        }
    }

    private void handleAllEvents()
    {
        if (!_events.Any()) return;

        foreach (var each in _events)
        {
            if (_routes.TryGetValue(each.GetType(), out var handlers))
            {
                Array.ForEach(handlers.ToArray(), action => action.Invoke(each));
            }
        }

        _events.Clear();
    }
}