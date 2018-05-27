namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IReadSlaveToMaster{TModel,TId}" />
    public interface IRudSlaveToMaster<TModel, TId> :
        IReadSlaveToMaster<TModel, TId>,
        IUpdateSlave<TModel, TId>,
        IDeleteSlave<TId>,
        IDeleteChildren<TId>
    {
    }
}
