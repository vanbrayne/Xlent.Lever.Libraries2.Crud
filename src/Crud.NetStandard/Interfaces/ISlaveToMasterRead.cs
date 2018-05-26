using Xlent.Lever.Libraries2.Core.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IReadChildren{TModel,TId}" />
    public interface ISlaveToMasterRead<TModel, in TId> :
        IReadChildren<TModel, TId>, 
        IReadSlave<TModel, TId>
    {
    }
}
