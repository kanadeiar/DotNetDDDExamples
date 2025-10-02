using ESCQRS.Catalog.Core.Base.Abstractions;
using Kanadeiar.Common.Functionals;

namespace ESCQRS.Catalog.Core.BrandAggregate.Values;

public record BrandId(Guid Value) : IId
{
    public static BrandId New() => new(Guid.NewGuid());

    public Guid Value { get; } = Value.Require(Value != Guid.Empty, () =>
        throw new ApplicationException("Номер идентификатора должен быть назначен"));

    public override string ToString() => Value.ToString();
}