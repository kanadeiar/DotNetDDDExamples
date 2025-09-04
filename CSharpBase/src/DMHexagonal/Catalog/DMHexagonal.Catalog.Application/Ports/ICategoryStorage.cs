using DMHexagonal.Catalog.Application.Ports.Base;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;

namespace DMHexagonal.Catalog.Application.Ports;

public interface ICategoryStorage : ITransactionalStorage
{
    public IEnumerable<CategoryItem> Load(Predicate<CategoryEntry> predicate);

    CategoryItem Load(CategoryId id);

    CategoryId Save(CategoryItem aggregate);
}