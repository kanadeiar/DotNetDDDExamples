using DMHexagonal.Catalog.Application.Ports.Base;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;

namespace DMHexagonal.Catalog.Application.Ports;

public interface ICategoryStorage : ITransactionalStorage
{
    CategoryId NextIdentity();

    public IEnumerable<CategoryItem> All();

    CategoryItem Load(CategoryId id);

    void Save(CategoryItem aggregate);
}