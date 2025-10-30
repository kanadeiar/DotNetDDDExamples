using Kanadeiar.Common.Functionals;

namespace ESCQRS.Catalog.Core.BrandAggregate.Values;

public record BrandNameValue(string Value)
{
    public string Value { get; } = Value.Require(Value.Length is >= 3 and <= 300, () =>
        throw new ApplicationException("Название бренда товаров должно быть длинной от 3 до 300 символов"));

    public override string ToString() => $"{Value}";
}