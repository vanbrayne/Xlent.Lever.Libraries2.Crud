using System.Threading;
using System.Threading.Tasks;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IManyToOneCrd{TManyModelCreate, TManyModel,TId}" />
    public interface IManyToOneCrd<TManyModel, TId> : 
        IManyToOneCrd<TManyModel, TManyModel, TId>,
        ICrd<TManyModel, TId>
    {
    }

    /// <inheritdoc cref="IManyToOneRead{TManyModel,TId}" />
    public interface IManyToOneCrd<in TManyModelCreate, TManyModel, TId> :
        IManyToOneRead<TManyModel, TId>,
        ICrd<TManyModelCreate, TManyModel, TId>,
        IDeleteChildren<TId>
        where TManyModel : TManyModelCreate
    {
    }
}
