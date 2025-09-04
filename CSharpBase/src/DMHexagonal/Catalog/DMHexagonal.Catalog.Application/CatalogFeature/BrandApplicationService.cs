using DMHexagonal.Catalog.Application.Ports;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Application.CatalogFeature;

public class BrandApplicationService(IBrandStorage storage)
{
    public Result<IEnumerable<BrandItem>> AllItems()
    {
        try
        {
            var items = storage.Load(e => !e.IsDeleted);

            return Result.Ok(items);
        }
        catch (Exception e)
        {
            return Result.Fail<IEnumerable<BrandItem>>("Не удалось получить все элементы. Ошибка: " + e);
        }
    }

    public Result<BrandId> AddItem(BrandNameValue name)
    {
        try
        {
            storage.BeginTransaction();

            var item = BrandItem.Create(BrandId.New(), name);

            var id = storage.Save(item);
            storage.Commit();

            return Result.Ok(id);
        }
        catch (Exception e)
        {
            storage.Rollback();
            return Result.Fail<BrandId>("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    public Result ChangeName(BrandId id, BrandNameValue newName)
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

    public Result DeleteItem(BrandId id)
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