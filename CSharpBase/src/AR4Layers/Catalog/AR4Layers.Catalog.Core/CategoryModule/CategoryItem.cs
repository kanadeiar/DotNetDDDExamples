using AR4Layers.Catalog.DataAccess;
using AR4Layers.Catalog.DataAccess.Entries;
using AR4Layers.Catalog.DataAccess.Registries;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Core.CategoryModule;

public class CategoryItem(int id, string name, bool isDeleted = false)
{
    private string _name = name
        .Require(name!.Length is >= 3 and <= 300, () => throw new ApplicationException("Название категории товаров должно быть приемлемой длинны"));
    private bool _isDeleted = isDeleted;

    public int Id { get; } = id
        .Require(id != 0, () => throw new ApplicationException("Идентификатор должен быть задан"));

    public static CategoryItem Create(string name)
    {
        var id = DataRegistry.ProductStorage.NextIdentity();

        return new CategoryItem(id, name);
    }

    public static CategoryItem Restore(CategoryEntry entry)
    {
        return new CategoryItem(entry.Id, entry.Name, entry.IsDeleted);
    }
    
    public void Rename(string newName)
    {
        if (newName.Length is >= 3 and <= 300 == false) throw new ApplicationException("Новое название категории товаров должно быть приемлемой длинны");

        _name = newName;
    }

    public void Delete()
    {
        _isDeleted = true;
    }

    #region ActiveRecord

    public static Result<CategoryItem> Find(int id)
    {
        try
        {
            var entry = DataRegistry.BrandStorage.Load(id);
            if (entry is null) return Result.Fail<CategoryItem>($"Элемент с идентификатором {id} не найден");

            var result = new CategoryItem(entry.Id,
                entry.Name,
                entry.IsDeleted);

            return Result.Ok(result);
        }
        catch (Exception e)
        {
            return Result.Fail<CategoryItem>("Не удалось найти элемент. Ошибка: " + e);
        }
    }

    public Result Add()
    {
        try
        {
            var entity = Entry();

            DataRegistry.CategoryStorage.Save(entity);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    public Result Save()
    {
        try
        {
            var entry = DataRegistry.CategoryStorage.Load(Id);
            if (entry is null) return Result.Fail($"Элемент с идентификатором {Id} не найден");

            var entity = this.Entry();

            DataRegistry.CategoryStorage.Save(entity);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    #endregion

    public CategoryEntry Entry() =>
        new()
        {
            Id = id,
            Name = _name,
            IsDeleted = _isDeleted,
        };

    public override string ToString() => $"{_name}";
}