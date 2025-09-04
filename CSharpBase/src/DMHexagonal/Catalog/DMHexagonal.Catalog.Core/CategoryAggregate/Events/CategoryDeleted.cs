using DMHexagonal.Catalog.Core.Base;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;

namespace DMHexagonal.Catalog.Core.CategoryAggregate.Events;

public record CategoryDeleted(CategoryId Id) : DomainEvent;