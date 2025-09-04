using AR4Layers.Catalog.Core.ProductModule;
using AR4Layers.Catalog.DataAccess.Registries;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Services.Services;

public class ProductApplicationService
{
    public Result<IEnumerable<ProductItem>> AllItems()
    {
        try
        {
            var items = DataRegistry.ProductStorage.Load(e => !e.IsDeleted).Select(ProductItem.Restore);

            return Result.Ok(items);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<ProductItem>>("Не удалось получить все элементы. Ошибка: " + e);
        }
    }

    public Result<int> AddItem(string name, decimal price, int brandId, int categoryId)
    {
        try
        {
            DataRegistry.ProductStorage.BeginTransaction();

            var item = ProductItem.Create(name, price, brandId, categoryId);
            var result = item.Add();

            DataRegistry.ProductStorage.Commit();
            return result switch
            {
                IFail fail => Result.Fail<int>(fail.Error),
                not null => Result.Ok(item.Id),
                _ => throw new IndexOutOfRangeException(nameof(result)),
            };
        }
        catch (Exception e)
        {
            DataRegistry.ProductStorage.Rollback();
            return Result.Fail<int>("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    public Result ChangeName(int id, string newName)
    {
        try
        {
            DataRegistry.ProductStorage.BeginTransaction();
            var entry = DataRegistry.ProductStorage.Load(id);
            if (entry == null) return Result.Fail("Не удалось найти элемент.");
            var item = ProductItem.Restore(entry);

            item.Rename(newName);

            item.Save();
            DataRegistry.ProductStorage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            DataRegistry.ProductStorage.Rollback();
            return Result.Fail("Не удалось изменить название элемента. Ошибка: " + e);
        }
    }

    public Result ChangePrice(int id, decimal newPrice)
    {
        try
        {
            DataRegistry.ProductStorage.BeginTransaction();
            var entry = DataRegistry.ProductStorage.Load(id);
            if (entry == null) return Result.Fail("Не удалось найти элемент.");
            var item = ProductItem.Restore(entry);

            item.ChangePrice(newPrice);

            item.Save();
            DataRegistry.ProductStorage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            DataRegistry.ProductStorage.Rollback();
            return Result.Fail("Не удалось изменить цену элемента. Ошибка: " + e);
        }
    }

    public Result DeleteItem(int id)
    {
        try
        {
            DataRegistry.ProductStorage.BeginTransaction();
            var entry = DataRegistry.ProductStorage.Load(id);
            if (entry == null) return Result.Fail("Не удалось найти элемент.");
            var item = ProductItem.Restore(entry);

            item.Delete();

            item.Save();
            DataRegistry.ProductStorage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            DataRegistry.ProductStorage.Rollback();
            return Result.Fail("Не удалось удалить элемент. Ошибка: " + e);
        }
    }
}