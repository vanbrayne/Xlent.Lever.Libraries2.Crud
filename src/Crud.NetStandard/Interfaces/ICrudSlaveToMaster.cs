namespace Xlent.Lever.Libraries2.Crud.Interfaces
{

    /// <inheritdoc cref="ICrudSlaveToMaster{TModelCreate,TModel,TId}" />
    public interface ICrudSlaveToMaster<TModel, TId> :
        ICrudSlaveToMaster<TModel, TModel, TId>,
        ICrdSlaveToMaster<TModel, TId>
    {
    }

    /// <inheritdoc cref="ICrdSlaveToMaster{TModelCreate,TModel,TId}" />
    public interface ICrudSlaveToMaster<in TModelCreate, TModel, TId> :
        ICrdSlaveToMaster<TModelCreate, TModel, TId>,
        IRudSlaveToMaster<TModel, TId>
        where TModel : TModelCreate
    {
    }
}
