using DMHexagonal.Catalog.Application.Ports.Base;
using DMHexagonal.Catalog.Core.Entries;
using DMHexagonal.Catalog.Core.ProductAggregate;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;

namespace DMHexagonal.Catalog.Application.Ports;

public interface IProductStorage : ITransactionalStorage
{
    public IEnumerable<ProductItem> Load(Predicate<ProductEntry> predicate);

    ProductItem Load(ProductId id);

    ProductId Save(ProductItem aggregate);
}