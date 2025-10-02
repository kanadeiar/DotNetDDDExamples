namespace ESCQRS.Catalog.Core.Base.Abstractions;

public interface IEventStore
{
    EventStream LoadEventStream(IId id);

    void AppendToStream(IId id, ICollection<IMessage> events, int expectedVersion);
}
