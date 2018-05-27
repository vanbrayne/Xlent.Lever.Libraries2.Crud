using Xlent.Lever.Libraries2.Core.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IReadChildren{TModel,TId}" />
    public interface IReadSlaveToMaster<TModel, TId> :
        IReadChildren<TModel, TId>, 
        IReadSlave<TModel, TId>,
        IRead<TModel, SlaveToMasterId<TId>>
    {
    }
}
