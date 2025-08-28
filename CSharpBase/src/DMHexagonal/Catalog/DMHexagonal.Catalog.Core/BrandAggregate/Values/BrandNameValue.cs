using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Core.BrandAggregate.Values;

public record BrandNameValue(string Name)
{
    public string Name { get; } = Name.Require(Name.Length is >= 3 and <= 300, () =>
        throw new ApplicationException("Название бренда товаров должно быть длинной от 3 до 300 символов"));

    public override string ToString() => $"{Name}";
}