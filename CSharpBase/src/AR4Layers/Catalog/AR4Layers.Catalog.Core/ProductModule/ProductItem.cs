using AR4Layers.Catalog.DataAccess.Entries;
using AR4Layers.Catalog.DataAccess.Registries;
using Kanadeiar.Common.Functionals;

namespace AR4Layers.Catalog.Core.ProductModule;

public class ProductItem(int id, string name, decimal price, int brandId, int categoryId, bool isDeleted = false)
{
    private string _name = name
        .Require(name!.Length is >= 3 and <= 290, () => throw new ApplicationException("Название товара должно быть приемлемой длинны"));
    private decimal _price = price
        .Require(price is >= 0 and <= 10000, () => throw new ApplicationException("Цена товара должна быть установлена от 0 до 10000"));
    private readonly int _brandId = brandId
        .Require(brandId != 0, () => throw new ApplicationException("Идентификатор бренда должен быть задан"));
    private readonly int _categoryId = categoryId
        .Require(categoryId != 0, () => throw new ApplicationException("Идентификатор каталога должен быть задан"));
    private bool _isDeleted = isDeleted;

    public int Id { get; } = id
        .Require(id != 0, () => throw new ApplicationException("Идентификатор должен быть задан"));

    public static ProductItem Create(string name, decimal price, int brandId, int categoryId)
    {
        var id = DataRegistry.ProductStorage.NextIdentity();

        return new ProductItem(id, name, price, brandId, categoryId);
    }

    public static ProductItem Restore(ProductEntry entry)
    {
        return new ProductItem(entry.Id, entry.Name, entry.Price, entry.BrandId, entry.CategoryId, entry.IsDeleted);
    }

    public void Rename(string newName)
    {
        if (newName.Length is >= 3 and <= 300 == false) throw new ApplicationException("Новое название товара должно быть приемлемой длинны");

        _name = newName;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice is >= 0 and <= 10000 == false) throw new ApplicationException("Новая цена товара должна быть установлена от 0 до 10000");

        _price = newPrice;
    }

    public void Delete()
    {
        _isDeleted = true;
    }

    public bool Filter(string name = "", int brandId = 0, int categoryId = 0)
    {
        List<Func<bool>> actions = new ();
        if (string.IsNullOrEmpty(name) == false)
        {
            actions.Add(() => _name.StartsWith(name));
        }
        if (brandId > 0)
        {
            actions.Add(() => _brandId == brandId);
        }
        if (categoryId > 0)
        {
            actions.Add(() => _categoryId == categoryId);
        }

        return actions.All(act => act.Invoke());
    }

    #region ActiveRecord

    public static Result<ProductItem> Find(int id)
    {
        try
        {
            var entry = DataRegistry.ProductStorage.Load(id);
            if (entry is null) return Result.Fail<ProductItem>($"Элемент с идентификатором {id} не найден");

            var result = new ProductItem(entry.Id,
                entry.Name,
                entry.Price,
                entry.BrandId,
                entry.CategoryId,
                entry.IsDeleted);

            return Result.Ok(result);
        }
        catch (Exception e)
        {
            return Result.Fail<ProductItem>("Не удалось найти элемент. Ошибка: " + e);
        }
    }
    
    public Result Add()
    {
        try
        {
            var entity = Entry();

            DataRegistry.ProductStorage.Save(entity);
                
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
            var entry = DataRegistry.ProductStorage.Load(Id);
            if (entry is null) return Result.Fail($"Элемент с идентификатором {Id} не найден");

            var entity = this.Entry();

            DataRegistry.ProductStorage.Save(entity);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail("Не удалось добавить элемент. Ошибка: " + e);
        }
    }

    #endregion

    public ProductEntry Entry() =>
        new()
        {
            Id = id,
            Name = _name,
            Price = _price,
            BrandId = _brandId,
            CategoryId = _categoryId,
            IsDeleted = _isDeleted,
        };

    public override string ToString() => $"{_name}";

    public string FullText() => $"{_name} - {_price} руб.";
}