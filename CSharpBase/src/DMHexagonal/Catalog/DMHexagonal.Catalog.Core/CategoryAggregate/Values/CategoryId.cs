using DMHexagonal.Catalog.Core.Base.Abstractions;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Core.CategoryAggregate.Values;

public record CategoryId(int Value) : IId
{
    public int Value { get; } = Value.Require(Value >= 0, () =>
        throw new ApplicationException("Номер идентификатора должен быть положительным числом"));

    public override string ToString() => Value.ToString();
}