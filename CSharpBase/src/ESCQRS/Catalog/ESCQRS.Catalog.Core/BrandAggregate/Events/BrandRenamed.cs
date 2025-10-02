using ESCQRS.Catalog.Core.Base;
using ESCQRS.Catalog.Core.BrandAggregate.Values;

namespace ESCQRS.Catalog.Core.BrandAggregate.Events;

public record BrandRenamed(BrandId Id, BrandNameValue Name) : DomainEvent;
