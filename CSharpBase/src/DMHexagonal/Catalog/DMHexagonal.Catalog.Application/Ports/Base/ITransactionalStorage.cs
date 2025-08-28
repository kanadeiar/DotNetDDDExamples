namespace DMHexagonal.Catalog.Application.Ports.Base;

public interface ITransactionalStorage
{
    public void BeginTransaction();
    public void Commit();
    public void Rollback();
}