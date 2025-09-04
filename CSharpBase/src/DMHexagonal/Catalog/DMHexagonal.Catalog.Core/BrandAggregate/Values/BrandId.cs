using DMHexagonal.Catalog.Core.Base.Abstractions;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Core.BrandAggregate.Values;

public record BrandId(int Value) : IId
{
    public int Value { get; } = Value.Require(Value >= 0, () =>
        throw new ApplicationException("Номер идентификатора должен быть назначен"));

    public override string ToString() => Value.ToString();
}