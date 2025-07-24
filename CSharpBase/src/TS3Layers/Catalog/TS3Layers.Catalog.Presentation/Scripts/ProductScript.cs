using Kanadeiar.Common.Functionals;
using System.Xml.Linq;
using TS3Layers.Catalog.Core.BrandModule;
using TS3Layers.Catalog.Core.ProductModule;
using TS3Layers.Catalog.DataAccess.Data;

namespace TS3Layers.Catalog.Presentation.Scripts;

public class ProductScript(ProductStorage storage)
{
    public Result<IEnumerable<ProductItem>> AllItems()
    {
        var items = storage.All().Select(ProductItem.Restore);

        return Result.Ok(items);
    }

    public Result<int> AddItem(string name, int brandId, int categoryId, decimal price)
    {
        try
        {
            storage.BeginTransaction();
            var id = storage.NextIdentity();

            var item = new ProductItem(id, name, brandId, categoryId, price);

            storage.Save(item.Entry());
            storage.Commit();
            return Result.Ok(id);
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail<int>("Не удалось добавить новый элемент. Ошибка: " + e);
        }
    }

    public Result ChangeName(int id, string newName)
    {
        try
        {
            storage.BeginTransaction();
            var entry = storage.Load(id);
            if (entry is null) throw new ApplicationException("Элемент не найден");
            var item = ProductItem.Restore(entry);

            item.Rename(newName);

            storage.Save(item.Entry());
            storage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail("Не удалось изменить название элемента. Ошибка: " + e);
        }
    }

    public Result ChangePrice(int id, decimal newPrice)
    {
        try
        {
            storage.BeginTransaction();
            var entry = storage.Load(id);
            if (entry is null) throw new ApplicationException("Элемент не найден");
            var item = ProductItem.Restore(entry);

            item.ChangePrice(newPrice);

            storage.Save(item.Entry());
            storage.Commit();
            return Result.Ok();
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail("Не удалось изменить цену элемента. Ошибка: " + e);
        }
    }

    public Result DeleteItem(int id)
    {
        try
        {
            storage.BeginTransaction();
            var entry = storage.Load(id);
            if (entry is null) throw new ApplicationException("Элемент не найден для удаления");
            var item = ProductItem.Restore(entry);

            item.Delete();

            storage.Save(item.Entry());
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