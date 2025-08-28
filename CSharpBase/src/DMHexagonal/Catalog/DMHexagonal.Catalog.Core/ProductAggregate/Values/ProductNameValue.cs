using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Core.ProductAggregate.Values;

public record ProductNameValue(string Name)
{
    public string Name { get; } = Name.Require(Name.Length is >= 3 and <= 300, () =>
        throw new ApplicationException("Название категории товаров должно быть длинной от 3 до 300 символов"));

    public override string ToString() => $"{Name}";
}