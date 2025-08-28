using AR4Layers.Catalog.DataAccess.Entries;
using AR4Layers.Catalog.DataAccess.Registries;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Core.BrandModule;

public class BrandItem(int id, string name, bool isDeleted = false)
{
    private string _name = name
        .Require(name!.Length is >= 3 and <= 300, () => throw new ApplicationException("Название бренда должно быть приемлемой длинны"));
    private bool _isDeleted = isDeleted;

    public int Id { get; } = id
        .Require(id != 0, () => throw new ApplicationException("Идентификатор должен быть задан"));

    public static BrandItem Create(string name)
    {
        var id = DataRegistry.BrandStorage.NextIdentity();

        return new BrandItem(id, name);
    }

    public static BrandItem Restore(BrandEntry entry)
    {
        return new BrandItem(entry.Id, entry.Name, entry.IsDeleted);
    }

    public void Rename(string newName)
    {
        if (newName.Length is >= 3 and <= 300 == false) throw new ApplicationException("Новое название бренда должно быть приемлемой длинны");

        _name = newName;
    }

    public void Delete()
    {
        _isDeleted = true;
    }

    #region ActiveRecord

    public static Result<BrandItem> Find(int id)
    {
        try
        {
            var entry = DataRegistry.BrandStorage.Load(id);
            if (entry is null) return Result.Fail<BrandItem>($"Элемент с идентификатором {id} не найден");

            var result = new BrandItem(entry.Id,
                entry.Name,
                entry.IsDeleted);

            return Result.Ok(result);
        }
        catch (Exception e)
        {
            return Result.Fail<BrandItem>("Не удалось найти элемент. Ошибка: " + e);
        }
    }

    public Result Add()
    {
        try
        {
            var entity = Entry();

            DataRegistry.BrandStorage.Save(entity);

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
            var entry = DataRegistry.BrandStorage.Load(Id);
            if (entry is null) return Result.Fail($"Элемент с идентификатором {Id} не найден");

            var entity = this.Entry();

            DataRegistry.BrandStorage.Save(entity);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    #endregion

    public BrandEntry Entry() =>
        new()
        {
            Id = Id,
            Name = _name,
            IsDeleted = _isDeleted,
        };

    public override string ToString() => $"{_name}";
}