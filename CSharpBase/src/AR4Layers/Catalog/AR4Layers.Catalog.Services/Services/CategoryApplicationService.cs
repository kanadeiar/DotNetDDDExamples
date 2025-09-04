using AR4Layers.Catalog.Core.CategoryModule;
using AR4Layers.Catalog.DataAccess.Registries;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Services.Services;

public class CategoryApplicationService
{
    public Result<IEnumerable<CategoryItem>> AllItems()
    {
        try
        {
            var items = DataRegistry.CategoryStorage.Load(e => !e.IsDeleted).Select(CategoryItem.Restore);

            return Result.Ok(items);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<CategoryItem>>("Не удалось получить все элементы. Ошибка: " + e);
        }
    }

    public Result<int> AddItem(string name)
    {
        try
        {
            DataRegistry.CategoryStorage.BeginTransaction();

            var item = CategoryItem.Create(name);
            var result = item.Add();

            DataRegistry.CategoryStorage.Commit();
            return result switch
            {
                IFail fail => Result.Fail<int>(fail.Error),
                not null => Result.Ok(item.Id),
                _ => throw new IndexOutOfRangeException(nameof(result)),
            };
        }
        catch (Exception e)
        {
            DataRegistry.CategoryStorage.Rollback();
            return Result.Fail<int>("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    public Result ChangeName(int id, string newName)
    {
        try
        {
            DataRegistry.CategoryStorage.BeginTransaction();
            var entry = DataRegistry.CategoryStorage.Load(id);
            if (entry == null) return Result.Fail("Не удалось найти элемент.");
            var item = CategoryItem.Restore(entry);

            item.Rename(newName);

            item.Save();
            DataRegistry.CategoryStorage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            DataRegistry.CategoryStorage.Rollback();
            return Result.Fail("Не удалось изменить название элемента. Ошибка: " + e);
        }
    }

    public Result DeleteItem(int id)
    {
        try
        {
            DataRegistry.CategoryStorage.BeginTransaction();
            var entry = DataRegistry.CategoryStorage.Load(id);
            if (entry == null) return Result.Fail("Не удалось найти элемент.");
            var item = CategoryItem.Restore(entry);

            item.Delete();

            item.Save();
            DataRegistry.CategoryStorage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            DataRegistry.CategoryStorage.Rollback();
            return Result.Fail("Не удалось удалить элемент. Ошибка: " + e);
        }
    }
}