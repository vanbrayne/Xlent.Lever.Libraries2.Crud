using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ISlaveToMaster{TModel,TId}" />
    public interface ISlaveToMasterRead<TModel, TId> :
        ISlaveToMaster<TModel, TId>, 
        IRead<TModel, SlaveToMasterId<TId>>
    {
    }
}
