using System.Collections.Generic;
using System.Linq;
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
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class ReadAutoCache<TModel, TId> : AutoCacheBase<TModel, TId>, IRead<TModel, TId>
    {
        private readonly IRead<TModel, TId> _storage;
        private const string ReadAllCacheKey = "ReadAllCacheKey";

        /// <summary>
        /// Constructor for TModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public ReadAutoCache(IRead<TModel, TId> storage, IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
        : this(storage, item => ((IUniquelyIdentifiable<TId>)item).Id, cache, flushCacheDelegateAsync, options)
        {
        }


        /// <summary>
        /// Constructor for TModel that does not implement <see cref="IUniquelyIdentifiable{TId}"/>, or when you want to specify your own GetKey() method.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="getIdDelegate"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public ReadAutoCache(IRead<TModel, TId> storage, GetIdDelegate<TModel, TId> getIdDelegate, IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null) 
            : base(getIdDelegate, cache, flushCacheDelegateAsync, options)
        {
            InternalContract.RequireNotNull(storage, nameof(storage));
            _storage = storage;
        }
    }
}
