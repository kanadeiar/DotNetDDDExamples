using DMHexagonal.Catalog.Core.Base;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;

namespace DMHexagonal.Catalog.Core.ProductAggregate.Events;

public record ProductRenamed(ProductId Id, ProductNameValue Name) : DomainEvent;