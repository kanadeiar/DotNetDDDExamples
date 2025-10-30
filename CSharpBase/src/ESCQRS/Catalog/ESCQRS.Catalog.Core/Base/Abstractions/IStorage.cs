namespace ESCQRS.Catalog.Core.Base.Abstractions;

public interface IStorage<T> : ITransactionalStorage
    where T : EventAggregateRoot, new()
{
    T Load(IId id);

    void Save(T aggregate);
}

public interface ITransactionalStorage
{
    public void BeginTransaction();
    public void Commit();
    public void Rollback();
}
