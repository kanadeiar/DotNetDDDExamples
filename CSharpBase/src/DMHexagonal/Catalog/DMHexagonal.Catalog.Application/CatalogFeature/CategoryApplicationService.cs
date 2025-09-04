using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Application.CatalogFeature;

public class CategoryApplicationService(ICategoryStorage storage)
{
    public Result<IEnumerable<CategoryItem>> AllItems()
    {
        try
        {
            var items = storage.Load(e => !e.IsDeleted);

            return Result.Ok(items);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<CategoryItem>>("Не удалось получить все элементы. Ошибка: " + e);
        }
    }

    public Result<CategoryId> AddItem(CategoryNameValue name)
    {
        try
        {
            storage.BeginTransaction();

            var item = CategoryItem.Create(CategoryId.New(), name);

            var id = storage.Save(item);
            storage.Commit();

            return Result.Ok(id);
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail<CategoryId>("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    public Result ChangeName(CategoryId id, CategoryNameValue newName)
    {
        try
        {
            storage.BeginTransaction();
            var item = storage.Load(id);

            item.Rename(newName);

            storage.Save(item);
            storage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail("Не удалось изменить название элемента. Ошибка: " + e);
        }
    }

    public Result DeleteItem(CategoryId id)
    {
        try
        {
            storage.BeginTransaction();
            var item = storage.Load(id);

            item.Delete();

            storage.Save(item);
            storage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail("Не удалось удалить элемент. Ошибка: " + e);
        }
    }
}