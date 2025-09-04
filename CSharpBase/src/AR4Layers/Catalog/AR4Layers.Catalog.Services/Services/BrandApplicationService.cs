using AR4Layers.Catalog.Core.BrandModule;
using AR4Layers.Catalog.DataAccess.Registries;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Services.Services;

public class BrandApplicationService
{
    public Result<IEnumerable<BrandItem>> AllItems()
    {
        try
        {
            var items = DataRegistry.BrandStorage.Load(e => !e.IsDeleted).Select(BrandItem.Restore);

            return Result.Ok(items);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<BrandItem>>("Не удалось получить все элементы. Ошибка: " + e);
        }
    }

    public Result<int> AddItem(string name)
    {
        try
        {
            DataRegistry.BrandStorage.BeginTransaction();

            var item = BrandItem.Create(name);
            var result = item.Add();

            DataRegistry.BrandStorage.Commit();
            return result switch
            {
                IFail fail => Result.Fail<int>(fail.Error),
                not null => Result.Ok(item.Id),
                _ => throw new IndexOutOfRangeException(nameof(result)),
            };
        }
        catch (Exception e)
        {
            DataRegistry.BrandStorage.Rollback();
            return Result.Fail<int>("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    public Result ChangeName(int id, string newName)
    {
        try
        {
            DataRegistry.BrandStorage.BeginTransaction();
            var entry = DataRegistry.BrandStorage.Load(id);
            if (entry == null) return Result.Fail("Не удалось найти элемент.");
            var item = BrandItem.Restore(entry);

            item.Rename(newName);

            item.Save();
            DataRegistry.BrandStorage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            DataRegistry.BrandStorage.Rollback();
            return Result.Fail("Не удалось изменить название элемента. Ошибка: " + e);
        }
    }

    public Result DeleteItem(int id)
    {
        try
        {
            DataRegistry.BrandStorage.BeginTransaction();
            var entry = DataRegistry.BrandStorage.Load(id);
            if (entry == null) return Result.Fail("Не удалось найти элемент.");
            var item = BrandItem.Restore(entry);

            item.Delete();

            item.Save();
            DataRegistry.BrandStorage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            DataRegistry.BrandStorage.Rollback();
            return Result.Fail("Не удалось удалить элемент. Ошибка: " + e);
        }
    }
}