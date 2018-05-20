using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Crud.Interfaces
{
    /// <inheritdoc cref="IManyToOne{TManyModel,TId}" />
    public interface IManyToOneRead<TManyModel, in TId> :
        IManyToOne<TManyModel, TId>,
        IRead<TManyModel, TId>
    {
    }
}
