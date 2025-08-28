using DMHexagonal.Catalog.Application.Ports.Base;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;

namespace DMHexagonal.Catalog.Application.Ports;

public interface IBrandStorage : ITransactionalStorage
{
    BrandId NextIdentity();

    public IEnumerable<BrandItem> All();

    BrandItem Load(BrandId id);

    void Save(BrandItem aggregate);
}