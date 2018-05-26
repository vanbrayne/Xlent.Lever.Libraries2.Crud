using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.Cache
{
    /// <summary>
    /// Use this to put an "intelligent" cache between you and your ICrud storage.
    /// </summary>
    /// <typeparam name="TModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId"></typeparam>
    public class CrudAutoCache<TModel, TId> : CrudAutoCache<TModel, TModel, TId>, ICrud<TModel, TId>
    {
        /// <summary>
        /// Constructor for TModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public CrudAutoCache(ICrud<TModel, TId> storage, IDistributedCache cache,
            FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
            : this(storage, item => ((IUniquelyIdentifiable<TId>)item).Id, cache, flushCacheDelegateAsync, options)
        {
        }


        /// <summary>
        /// Constructor for TModel that does not implement <see cref="IUniquelyIdentifiable{TId}"/>, or when you want to specify your own GetKey() method.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="cache"></param>
        /// <param name="getIdDelegate"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public CrudAutoCache(ICrud<TModel, TId> storage, GetIdDelegate<TModel, TId> getIdDelegate,
            IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null,
            AutoCacheOptions options = null)
            : base(storage, getIdDelegate, cache, flushCacheDelegateAsync, options)
        {
        }
    }

    /// <summary>
    /// Use this to put an "intelligent" cache between you and your ICrud storage.
    /// </summary>
    /// <typeparam name="TModelCreate">The type for creating objects in persistant storage.</typeparam>
    /// <typeparam name="TModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId"></typeparam>
    public class CrudAutoCache<TModelCreate, TModel, TId> : AutoCacheBase<TModel, TId>, ICrud<TModelCreate, TModel, TId> where TModel : TModelCreate
    {

        /// <summary>
        /// Constructor for TModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public CrudAutoCache(ICrud<TModelCreate, TModel, TId> storage, IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
        : this(storage, item => ((IUniquelyIdentifiable<TId>)item).Id, cache, flushCacheDelegateAsync, options)
        {
        }


        /// <summary>
        /// Constructor for TModel that does not implement <see cref="IUniquelyIdentifiable{TId}"/>, or when you want to specify your own GetKey() method.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="cache"></param>
        /// <param name="getIdDelegate"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public CrudAutoCache(ICrud<TModelCreate, TModel, TId> storage, GetIdDelegate<TModel, TId> getIdDelegate, IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
            : base(storage, getIdDelegate, cache, flushCacheDelegateAsync, options)
        {
            _storage = storage;
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(item, nameof(item));
            var createdItem = await _storage.CreateAndReturnAsync(item, token);
            await CacheSetAsync(createdItem, token);
            return createdItem;
        }

        /// <inheritdoc />
        public async Task<TId> CreateAsync(TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(item, nameof(item));
            var id = await _storage.CreateAsync(item, token);
            await CacheMaybeSetAsync(id, token);
            return id;
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(TId id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            InternalContract.RequireNotDefaultValue(item, nameof(item));
            var createdItem = await _storage.CreateWithSpecifiedIdAndReturnAsync(id, item, token);
            await CacheSetByIdAsync(id, createdItem, token);
            return createdItem;
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(TId id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            InternalContract.RequireNotDefaultValue(item, nameof(item));
            await _storage.CreateWithSpecifiedIdAsync(id, item, token);
            await CacheMaybeSetAsync(id, token);
        }

        /// <inheritdoc />
        public async Task<TModel> ReadAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            var item = await CacheGetByIdAsync(id, token);
            if (item != null) return item;
            item = await _storage.ReadAsync(id, token);
            if (Equals(item, default(TModel))) return default(TModel);
            await CacheSetByIdAsync(id, item, token);
            return item;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> ReadAllAsync(int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireGreaterThan(0, limit, nameof(limit));
            if (limit == 0) limit = int.MaxValue;
            var itemsArray = await CacheGetAsync(limit, ReadAllCacheKey, token);
            if (itemsArray != null) return itemsArray;
            var itemsCollection = await _storage.ReadAllAsync(limit, token);
            itemsArray = itemsCollection as TModel[] ?? itemsCollection.ToArray();
            CacheItemsInBackground(itemsArray, limit, ReadAllCacheKey);
            return itemsArray;
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            if (limit == null) limit = PageInfo.DefaultLimit;
            var result = await CacheGetAsync(offset, limit.Value, ReadAllCacheKey, token);
            if (result != null) return result;
            result = await _storage.ReadAllWithPagingAsync(offset, limit.Value, token);
            if (result?.Data == null) return null;
            CacheItemsInBackground(result, limit.Value, ReadAllCacheKey);
            return result;
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(TId id, TModel item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            InternalContract.RequireNotDefaultValue(item, nameof(item));
            var updatedItem = await _storage.UpdateAndReturnAsync(id, item, token);
            await CacheSetByIdAsync(id, updatedItem, token);
            return updatedItem;

        }

        /// <inheritdoc />
        public async Task UpdateAsync(TId id, TModel item, CancellationToken token = default(CancellationToken))
        {
            await _storage.UpdateAsync(id, item, token);
            await CacheMaybeSetAsync(id, token);
        }

        /// <inheritdoc />
        public async Task DeleteAllAsync(CancellationToken token = default(CancellationToken))
        {
            var task1 = FlushAsync(token);
            var task2 = _storage.DeleteAllAsync(token);
            await Task.WhenAll(task1, task2);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            var task1 = CacheRemoveByIdAsync(id, token);
            var task2 = _storage.DeleteAsync(id, token);
            await Task.WhenAll(task1, task2);
        }

        /// <inheritdoc />
        public Task<Lock> ClaimLockAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            return _storage.ClaimLockAsync(id, token);
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(Lock @lock, CancellationToken token = default(CancellationToken))
        {
            return _storage.ReleaseLockAsync(@lock, token);
        }

        /// <summary>
        /// Read and cache the item, but only if we have an aggressive caching strategy.
        /// </summary>
        /// <param name="id">The id of the item.</param>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        protected async Task CacheMaybeSetAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            async Task<bool> IsAlreadyCachedAndGetIsOkToUpdate()
            {
                return Options.DoGetToUpdate && await CacheItemExistsAsync(id, token);
            }

            var getAndSave = Options.SaveAll || await IsAlreadyCachedAndGetIsOkToUpdate();
            if (!getAndSave) return;
            var item = await _storage.ReadAsync(id, token);
            await CacheSetByIdAsync(id, item, token);
        }

        /// <summary>
        /// Check if an item exists in the cache.
        /// </summary>
        /// <param name="id">The id for the item</param>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        protected async Task<bool> CacheItemExistsAsync(TId id, CancellationToken token)
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            var key = GetCacheKeyFromId(id);
            var cachedItem = await Cache.GetAsync(key, token);
            return cachedItem != null;
        }
    }
}
