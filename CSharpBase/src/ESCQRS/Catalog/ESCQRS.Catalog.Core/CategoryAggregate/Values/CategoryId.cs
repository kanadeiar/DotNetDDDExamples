using ESCQRS.Catalog.Core.Base.Abstractions;
using Kanadeiar.Common.Functionals;

namespace ESCQRS.Catalog.Core.CategoryAggregate.Values;

public record CategoryId(Guid Value) : IId
{
    public static CategoryId New() => new(Guid.NewGuid());

    public Guid Value { get; } = Value.Require(Value != Guid.Empty, () =>
        throw new ApplicationException("Номер идентификатора должен быть назначен"));

    public override string ToString() => Value.ToString();
}