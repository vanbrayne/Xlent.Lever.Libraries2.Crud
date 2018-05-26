using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;

namespace Xlent.Lever.Libraries2.Crud.Cache
{
    /// <summary>
    /// Use this to put an "intelligent" cache between you and your ICrud storage.
    /// </summary>
    /// <typeparam name="TModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId"></typeparam>
    public class CrdAutoCache<TModel, TId> : CrdAutoCache<TModel, TModel, TId>, ICrd<TModel, TId>
    {

        /// <summary>
        /// Constructor for TModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public CrdAutoCache(ICrd<TModel, TId> storage, IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
            : base(storage, item => ((IUniquelyIdentifiable<TId>)item).Id, cache, flushCacheDelegateAsync, options)
        {
        }

    }

    /// <summary>
    /// Use this to put an "intelligent" cache between you and your ICrud storage.
    /// </summary>
    /// <typeparam name="TModelCreate">The type for creating objects in persistant storage.</typeparam>
    /// <typeparam name="TModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId"></typeparam>
    public class CrdAutoCache<TModelCreate, TModel, TId> : ReadAutoCache<TModel, TId>, ICrd<TModelCreate, TModel, TId> where TModel : TModelCreate
    {
        private readonly ICrd<TModelCreate, TModel, TId> _storage;

        /// <summary>
        /// Constructor for TModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public CrdAutoCache(ICrd<TModelCreate, TModel, TId> storage, IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
        : this(storage, item => ((IUniquelyIdentifiable<TId>)item).Id, cache, flushCacheDelegateAsync, options)
        {
        }
    }
}
