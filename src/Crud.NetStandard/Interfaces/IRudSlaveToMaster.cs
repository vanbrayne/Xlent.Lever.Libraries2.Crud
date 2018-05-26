namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ISlaveToMasterRead{TModel,TId}" />
    public interface IRudSlaveToMaster<TModel, in TId> :
        ISlaveToMasterRead<TModel, TId>,
        IUpdateSlave<TModel, TId>,
        IDeleteSlave<TId>,
        IDeleteChildren<TId>
    {
    }
}
