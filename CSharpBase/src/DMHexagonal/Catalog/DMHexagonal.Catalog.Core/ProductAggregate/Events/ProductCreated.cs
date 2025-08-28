using DMHexagonal.Catalog.Core.Base;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;

namespace DMHexagonal.Catalog.Core.ProductAggregate.Events;

public record ProductCreated(ProductId Id, ProductNameValue Name, PriceValue Price, BrandId BrandId, CategoryId CategoryId) : DomainEvent;