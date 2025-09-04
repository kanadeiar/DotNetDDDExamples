using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.ProductAggregate;
using DMHexagonal.Catalog.Core.ProductAggregate.Values;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Application.CatalogFeature;

public class ProductApplicationService(IProductStorage storage)
{
    public Result<IEnumerable<ProductItem>> AllItems()
    {
        try
        {
            var items = storage.Load(e => !e.IsDeleted);

            return Result.Ok(items);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<ProductItem>>("Не удалось получить все элементы. Ошибка: " + e);
        }
    }

    public Result<ProductId> AddItem(ProductNameValue name, PriceValue price, BrandId brand, CategoryId category)
    {
        try
        {
            storage.BeginTransaction();

            var item = ProductItem.Create(new ProductId(0), name, price, brand, category);

            var id = storage.Save(item);
            storage.Commit();

            return Result.Ok(id);
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail<ProductId>("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    public Result ChangeName(ProductId id, ProductNameValue newName)
    {
        try
        {
            storage.BeginTransaction();
            var item = storage.Load(id);
            if (item.Id.Value == 0) return Result.Fail("Не удалось найти элемент.");

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

    public Result ChangePrice(ProductId id, PriceValue newPrice)
    {
        try
        {
            storage.BeginTransaction();
            var item = storage.Load(id);
            if (item.Id.Value == 0) return Result.Fail("Не удалось найти элемент.");

            item.ChangePrice(newPrice);

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

    public Result DeleteItem(ProductId id)
    {
        try
        {
            storage.BeginTransaction();
            var item = storage.Load(id);
            if (item.Id.Value == 0) return Result.Fail("Не удалось найти элемент.");

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