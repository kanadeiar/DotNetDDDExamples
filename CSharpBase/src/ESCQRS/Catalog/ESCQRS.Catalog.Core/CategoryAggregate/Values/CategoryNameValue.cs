using Kanadeiar.Common.Functionals;

namespace ESCQRS.Catalog.Core.CategoryAggregate.Values;

public record CategoryNameValue(string Value)
{
    public string Value { get; } = Value.Require(Value.Length is >= 3 and <= 300, () =>
        throw new ApplicationException("Название категории товаров должно быть длинной от 3 до 300 символов"));

    public override string ToString() => $"{Value}";
}