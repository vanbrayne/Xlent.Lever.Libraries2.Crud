using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ISlaveToMasterRead{TModel,TId}" />
    public interface ISlaveToMasterRud<TModel, in TId> :
        ISlaveToMasterRead<TModel, TId>,
        IUpdateSlave<TModel, TId>,
        IDeleteSlave<TId>,
        IDeleteChildren<TId>
    {
    }
}
