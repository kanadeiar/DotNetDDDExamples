using DMHexagonal.Catalog.Application.Ports.Base;
using DMHexagonal.Catalog.Core.ProductAggregate;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;

namespace DMHexagonal.Catalog.Application.Ports;

public interface IProductStorage : ITransactionalStorage
{
    ProductId NextIdentity();

    public IEnumerable<ProductItem> All();

    ProductItem Load(ProductId id);

    void Save(ProductItem aggregate);
}