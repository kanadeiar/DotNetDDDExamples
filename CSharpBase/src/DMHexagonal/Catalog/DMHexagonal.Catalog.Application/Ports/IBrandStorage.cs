using DMHexagonal.Catalog.Application.Ports.Base;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;

namespace DMHexagonal.Catalog.Application.Ports;

public interface IBrandStorage : ITransactionalStorage
{
    public IEnumerable<BrandItem> Load(Predicate<BrandEntry> predicate);

    BrandItem Load(BrandId id);

    BrandId Save(BrandItem aggregate);
}