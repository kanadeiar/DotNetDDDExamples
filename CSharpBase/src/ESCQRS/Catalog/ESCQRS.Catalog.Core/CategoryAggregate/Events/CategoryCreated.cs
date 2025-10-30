using ESCQRS.Catalog.Core.Base;
using ESCQRS.Catalog.Core.CategoryAggregate.Values;

namespace ESCQRS.Catalog.Core.CategoryAggregate.Events;

public record CategoryCreated(CategoryId Id, CategoryNameValue Name) : DomainEvent;
