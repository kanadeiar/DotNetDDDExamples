using DMHexagonal.Catalog.Core.Base.Abstractions;
using Kanadeiar.Common.Functionals;

namespace DMHexagonal.Catalog.Core.BrandAggregate.Values;

public record BrandId(int Id) : IId
{
    public int Id { get; } = Id.Require(Id > 0, () =>
        throw new ApplicationException("Номер идентификатора должен быть положительным числом"));

    public override string ToString() => Id.ToString();
}