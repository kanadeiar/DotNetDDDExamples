namespace DMHexagonal.Catalog.Core.Base.Abstractions;

/// <summary>
/// Диспетчер доменных событий
/// </summary>
public interface IDispatcher : IRegisterDispatcher, IDispatchDispatcher;

/// <summary>
/// Диспетчер доменных событий, способный регистрировать обработчиков событий
/// </summary>
public interface IRegisterDispatcher
{
    void RegisterHandler<T>(Action<T> handler)
        where T : IMessage;
}

/// <summary>
/// Диспетчер доменных событий, способный публиковать события
/// </summary>
public interface IDispatchDispatcher
{
    public void Publish(IEnumerable<IMessage> events);
}