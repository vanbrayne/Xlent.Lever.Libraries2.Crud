namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ICrdSlaveToMaster{TModel,TId}" />
    public interface ICrdSlaveToMaster<TModel, TId> :
        ICrdSlaveToMaster<TModel, TModel, TId>,
        ICreateSlave<TModel, TId>,
        ICreateSlaveWithSpecifiedId<TModel, TId>
    {
    }

    /// <inheritdoc cref="ISlaveToMasterRead{TModel,TId}" />
    public interface ICrdSlaveToMaster<in TModelCreate, TModel, TId> :
        ISlaveToMasterRead<TModel, TId>,
        ICreateSlave<TModelCreate, TModel, TId>,
        ICreateSlaveWithSpecifiedId<TModelCreate, TModel, TId>,
        IDeleteSlave<TId>,
        IDeleteChildren<TId>
        where TModel : TModelCreate
    {
    }
}
