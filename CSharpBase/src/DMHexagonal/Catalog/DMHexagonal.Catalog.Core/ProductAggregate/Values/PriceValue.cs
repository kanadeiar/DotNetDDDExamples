using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Core.ProductAggregate.Values;

public record PriceValue(decimal Value)
{
    public decimal Value { get; } = Value.Require(Value is >= 0M and <= 100000M, () =>
        throw new ApplicationException("Цена товара должна быть от 0 до 100000 руб."));

    public override string ToString() => $"{Value}";
}