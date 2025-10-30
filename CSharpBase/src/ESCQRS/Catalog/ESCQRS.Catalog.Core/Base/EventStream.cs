using ESCQRS.Catalog.Core.Base.Abstractions;

namespace ESCQRS.Catalog.Core.Base;

public record EventStream(ICollection<IMessage> Events, int Version = -1);
