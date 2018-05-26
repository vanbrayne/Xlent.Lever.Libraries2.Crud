using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ISlaveToMasterCrd{TModel,TId}" />
    public interface ISlaveToMasterCrd<TModel, TId> :
        ISlaveToMasterCrd<TModel, TModel, TId>,
        ICreateSlave<TModel, TId>,
        ICreateSlaveWithSpecifiedId<TModel, TId>
    {
    }

    /// <inheritdoc cref="ISlaveToMasterRead{TModel,TId}" />
    public interface ISlaveToMasterCrd<in TModelCreate, TModel, TId> :
        ISlaveToMasterRead<TModel, TId>,
        ICreateSlave<TModelCreate, TModel, TId>,
        ICreateSlaveWithSpecifiedId<TModelCreate, TModel, TId>,
        IDeleteSlave<TId>,
        IDeleteChildren<TId>
        where TModel : TModelCreate
    {
    }
}
