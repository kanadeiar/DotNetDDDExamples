using DMHexagonal.Catalog.Core.Base;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;

namespace DMHexagonal.Catalog.Core.BrandAggregate.Events;

public record BrandRenamed(BrandId Id, BrandNameValue Name) : DomainEvent;