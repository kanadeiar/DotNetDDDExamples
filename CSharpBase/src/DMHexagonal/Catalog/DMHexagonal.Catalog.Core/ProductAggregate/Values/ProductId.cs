using DMHexagonal.Catalog.Core.Base.Abstractions;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Core.ProductAggregate.Values;

public record ProductId(Guid Value) : IId
{
    public static ProductId New() => new(Guid.NewGuid());

    public Guid Value { get; } = Value.Require(Value != Guid.Empty, () =>
        throw new ApplicationException("Номер идентификатора должен быть назначен"));

    public override string ToString() => Value.ToString();
}