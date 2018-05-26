using System.Threading;
using System.Threading.Tasks;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IManyToOneRead{TManyModel,TId}" />
    public interface IManyToOneRud<TManyModel, in TId> :
        IManyToOneRead<TManyModel, TId>,
        IRud<TManyModel, TId>,
        IDeleteChildren<TId>
    {
    }
}
