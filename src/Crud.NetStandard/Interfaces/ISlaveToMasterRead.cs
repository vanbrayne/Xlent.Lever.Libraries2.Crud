using Xlent.Lever.Libraries2.Core.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ISlaveToMaster{TModel,TId}" />
    public interface ISlaveToMasterRead<TModel, TId> :
        ISlaveToMaster<TModel, TId>, 
        IRead<TModel, SlaveToMasterId<TId>>
    {
    }
}
