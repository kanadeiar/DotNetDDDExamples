using Kanadeiar.Common.Functionals;
using TSEasy.Catalog.Core;
using TSEasy.Catalog.DataAccess;

namespace TSEasy.Catalog.Presentation;

public class ProductScript(ProductStorage storage)
{
    public Result<IEnumerable<ProductItem>> AllItems()
    {
        var items = storage.Load(e => !e.IsDeleted).Select(ProductItem.Restore);

        return Result.Ok(items);
    }

    public Result<IEnumerable<ProductItem>> Filter(string name = "", int brandId = 0, int categoryId = 0)
    {
        var query = storage.Load(e => !e.IsDeleted).AsQueryable();
        if (string.IsNullOrEmpty(name) == false)
        {
            query = query.Where(p => p.Name.StartsWith(name));
        }
        query = brandId > 0
            ? query.Where(p => p.BrandId == brandId)
            : query;
        query = categoryId > 0
            ? query.Where(p => p.CategoryId == categoryId)
            : query;

        var items = query.AsEnumerable().Select(ProductItem.Restore);

        return Result.Ok(items);
    }

    public Result<int> AddItem(string name, decimal price, int brandId, int categoryId)
    {
        try
        {
            storage.BeginTransaction();

            var item = new ProductItem(0, name, price, brandId, categoryId);

            var entry = item.Entry();
            storage.Save(entry);
            storage.Commit();
            return Result.Ok(entry.Id);
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