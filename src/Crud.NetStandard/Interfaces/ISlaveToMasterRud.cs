using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ISlaveToMasterRead{TModel,TId}" />
    public interface ISlaveToMasterRud<TModel, TId> :
        ISlaveToMasterRead<TModel, TId>, 
        IRud<TModel, SlaveToMasterId<TId>>
    {
        /// <summary>
        /// Delete all child items for a specific parent, <paramref name="parentId"/>.
        /// </summary>
        Task DeleteChildrenAsync(TId parentId, CancellationToken token = default(CancellationToken));
    }
}
