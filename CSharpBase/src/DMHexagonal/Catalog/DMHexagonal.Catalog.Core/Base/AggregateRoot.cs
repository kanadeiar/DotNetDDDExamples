using DMHexagonal.Catalog.Core.Base.Abstractions;

namespace DMHexagonal.Catalog.Core.Base;

public abstract class AggregateRoot
{
    private readonly List<IMessage> _changes = new();

    public abstract IId Id { get; }

    protected void ApplyChange(IMessage @event)
    {
        _changes.Add(@event);
    }

    public IEnumerable<IMessage> Changes()
    {
        var results = _changes.ToArray();
        _changes.Clear();

        return results;
    }
}