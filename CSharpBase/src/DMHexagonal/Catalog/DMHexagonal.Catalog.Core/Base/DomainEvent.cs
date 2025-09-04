using DMHexagonal.Catalog.Core.Base.Abstractions;

namespace DMHexagonal.Catalog.Core.Base;

public record DomainEvent : IMessage
{
    public DateTime OccurredOn { get; set; } = DateTime.Now;
}