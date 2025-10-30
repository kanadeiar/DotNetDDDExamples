using ESCQRS.Catalog.Core.BrandAggregate.Events;
using ESCQRS.Catalog.Core.BrandAggregate.Values;

namespace ESCQRS.Catalog.Core.ReadModel;

public record BrandProjection(BrandId Id, BrandNameValue Name, bool IsDeleted = false)
{
    public BrandProjection(BrandCreated ev) : this(ev.Id, ev.Name) { }

    public BrandProjection Apply(BrandRenamed ev) =>
        this with { Name = ev.Name };

    public BrandProjection Apply(BrandDeleted ev) =>
        this with { IsDeleted = true };

    public override string ToString() => $"{Name}";
}