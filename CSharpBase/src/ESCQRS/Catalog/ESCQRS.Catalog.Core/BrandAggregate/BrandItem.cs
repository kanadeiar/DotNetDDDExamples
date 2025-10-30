using ESCQRS.Catalog.Core.Base;
using ESCQRS.Catalog.Core.Base.Abstractions;
using ESCQRS.Catalog.Core.BrandAggregate.Events;
using ESCQRS.Catalog.Core.BrandAggregate.Values;

namespace ESCQRS.Catalog.Core.BrandAggregate;

public class BrandItem : EventAggregateRoot
{
    private BrandId _id = null!;
    public override BrandId Id => _id;

    public static BrandItem Create(BrandId id, BrandNameValue name)
    {
        var result = new BrandItem();
        result.ApplyChange(new BrandCreated(id, name));
        return result;
    }

    public void Rename(BrandNameValue newName)
    {
        ApplyChange(new BrandRenamed(Id, newName));
    }

    public void Delete()
    {
        ApplyChange(new BrandDeleted(Id));
    }

    protected override void Mutate(IMessage @event) =>
        ((dynamic)this).when((dynamic)@event);

    private void when(BrandCreated ev)
    {
        _id = ev.Id;
    }

    private void when(BrandRenamed ev) { }

    private void when(BrandDeleted ev) { }
}