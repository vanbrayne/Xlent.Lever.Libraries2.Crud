using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <summary>
    /// Functionality for persisting many-to-many relations.
    /// </summary>
    /// <typeparam name="TManyToManyModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TReferenceModel1"></typeparam>
    /// <typeparam name="TReferenceModel2"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface ICrudManyToMany<TManyToManyModel, TReferenceModel1, TReferenceModel2, TId> :
        ICrudManyToMany<TManyToManyModel, TManyToManyModel, TReferenceModel1, TReferenceModel2, TId>,
        ICrud<TManyToManyModel, TId>,
        IManyToMany<TManyToManyModel, TReferenceModel1, TReferenceModel2, TId>
    {
    }

    /// <summary>
    /// Functionality for persisting many-to-many relations.
    /// </summary>
    public interface ICrudManyToMany<in TManyToManyModelCreate, TManyToManyModel, TReferenceModel1, TReferenceModel2, TId> :
        ICrud<TManyToManyModelCreate, TManyToManyModel, TId>,
        IManyToMany<TManyToManyModelCreate, TManyToManyModel, TReferenceModel1, TReferenceModel2, TId>
        where TManyToManyModel : TManyToManyModelCreate
    {
    }
}
