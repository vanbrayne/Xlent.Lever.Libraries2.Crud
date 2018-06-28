﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Assert;
using Microsoft.Extensions.Caching.Distributed;
using Xlent.Lever.Libraries2.Core.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.Cache
{
    /// <inheritdoc cref="SlaveToMasterAutoCache{TManyModelCreate,TManyModel,TId}" />
    public class SlaveToMasterAutoCache<TManyModel, TId> :
        SlaveToMasterAutoCache<TManyModel, TManyModel, TId>, 
        ICrudSlaveToMaster<TManyModel, TId>
    {
        /// <summary>
        /// Constructor for TOneModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public SlaveToMasterAutoCache(ICrudable<TManyModel, TId> service,
            IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null,
            AutoCacheOptions options = null)
            : this(service, item => ((IUniquelyIdentifiable<SlaveToMasterId<TId>>)item).Id, cache,
                flushCacheDelegateAsync, options)
        {
        }


        /// <summary>
        /// Constructor for TOneModel that does not implement <see cref="IUniquelyIdentifiable{TId}"/>, or when you want to specify your own GetKey() method.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cache"></param>
        /// <param name="getIdDelegate"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public SlaveToMasterAutoCache(ICrudable<TManyModel, TId> service,
            GetIdDelegate<TManyModel, SlaveToMasterId<TId>> getIdDelegate, IDistributedCache cache,
            FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
            : base(service, getIdDelegate, cache, flushCacheDelegateAsync, options)
        {
        }
    }

    /// <summary>
    /// Use this to put an "intelligent" cache between you and your ICrud service.
    /// </summary>
    /// <typeparam name="TManyModelCreate">The model to use when creating objects.</typeparam>
    /// <typeparam name="TManyModel">The model for the children that each points out a parent.</typeparam>
    /// <typeparam name="TId">The type for the id field of the models.</typeparam>
    public class SlaveToMasterAutoCache<TManyModelCreate, TManyModel, TId> : 
        AutoCacheBase<TManyModel, SlaveToMasterId<TId>>, 
        ICrudSlaveToMaster<TManyModelCreate, TManyModel, TId>, 
        IRead<TManyModel, SlaveToMasterId<TId>> 
        where TManyModel : TManyModelCreate
    {
        private readonly ICrudSlaveToMaster<TManyModelCreate, TManyModel, TId> _service;
        /// <summary>
        /// Constructor for TOneModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public SlaveToMasterAutoCache(ICrudable<TManyModel, TId> service, IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
        : this(service, item => ((IUniquelyIdentifiable<SlaveToMasterId<TId>>)item).Id, cache, flushCacheDelegateAsync, options)
        {
        }


        /// <summary>
        /// Constructor for TOneModel that does not implement <see cref="IUniquelyIdentifiable{TId}"/>, or when you want to specify your own GetKey() method.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cache"></param>
        /// <param name="getIdDelegate"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public SlaveToMasterAutoCache(ICrudable<TManyModel, TId> service, GetIdDelegate<TManyModel, SlaveToMasterId<TId>> getIdDelegate, IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
            : base(getIdDelegate, cache, flushCacheDelegateAsync, options)
        {
            InternalContract.RequireNotNull(service, nameof(service));
            InternalContract.RequireNotNull(getIdDelegate, nameof(getIdDelegate));
            InternalContract.RequireNotNull(cache, nameof(cache));
            _service = new SlaveToMasterPassThrough<TManyModelCreate, TManyModel, TId>(service);
        }

        /// <summary>
        /// True while a background thread is active saving results from a ReadAll() operation.
        /// </summary>
        public bool IsCollectionOperationActive(TId parentId)
        {
            InternalContract.RequireNotDefaultValue(parentId, nameof(parentId));
            return IsCollectionOperationActive(CacheKeyForChildrenCollection(parentId));
        }

        private async Task RemoveCachedChildrenInBackgroundAsync(TId parentId, CancellationToken token = default(CancellationToken))
        {
            var key = CacheKeyForChildrenCollection(parentId);
            await RemoveCacheItemsInBackgroundAsync(key, async () => await CacheGetAsync(int.MaxValue, key, token));
        }

        /// <inheritdoc />
        public async Task<TId> CreateAsync(TId masterId, TManyModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var slaveId = await _service.CreateAsync(masterId, item, token);
            var id = new SlaveToMasterId<TId>(masterId, slaveId);
            await CacheMaybeSetAsync(id, _service, token);
            return slaveId;
        }

        /// <inheritdoc />
        public async Task<TManyModel> CreateAndReturnAsync(TId masterId, TManyModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var result = await _service.CreateAndReturnAsync(masterId, item, token);
            await CacheSetAsync(result, token);
            return result;
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(TId masterId, TId slaveId, TManyModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            await _service.CreateWithSpecifiedIdAsync(masterId, slaveId, item, token);
            var id = new SlaveToMasterId<TId>(masterId, slaveId);
            await CacheMaybeSetAsync(id, _service, token);
        }

        /// <inheritdoc />
        public async Task<TManyModel> CreateWithSpecifiedIdAndReturnAsync(TId masterId, TId slaveId, TManyModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var result = await _service.CreateWithSpecifiedIdAndReturnAsync(masterId, slaveId, item, token);
            await CacheSetAsync(result, token);
            return result;
        }

        /// <inheritdoc />
        public async Task<TManyModel> ReadAsync(TId masterId, TId slaveId, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(masterId, nameof(masterId));
            InternalContract.RequireNotDefaultValue(slaveId, nameof(slaveId));
            var id = new SlaveToMasterId<TId>(masterId, slaveId);
            var item = await CacheGetByIdAsync(id, token);
            if (item != null) return item;
            item = await _service.ReadAsync(masterId, slaveId, token);
            if (Equals(item, default(TManyModel))) return default(TManyModel);
            await CacheSetByIdAsync(id, item, token);
            return item;
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TManyModel>> ReadChildrenWithPagingAsync(TId parentId, int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(parentId, nameof(parentId));
            InternalContract.RequireGreaterThanOrEqualTo(0, offset, nameof(limit));
            if (limit == null) limit = PageInfo.DefaultLimit;
            InternalContract.RequireGreaterThan(0, limit.Value, nameof(limit));
            var key = CacheKeyForChildrenCollection(parentId);
            var result = await CacheGetAsync(offset, limit.Value, key, token);
            if (result != null) return result;
            result = await _service.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
            if (result?.Data == null) return null;
            CacheItemsInBackground(result, limit.Value, key);
            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TManyModel>> ReadChildrenAsync(TId masterId, int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(masterId, nameof(masterId));
            InternalContract.RequireGreaterThan(0, limit, nameof(limit));
            var key = CacheKeyForChildrenCollection(masterId);
            var itemsArray = await CacheGetAsync(limit, key, token);
            if (itemsArray != null) return itemsArray;
            var itemsCollection = await _service.ReadChildrenAsync(masterId, limit, token);
            itemsArray = itemsCollection as TManyModel[] ?? itemsCollection.ToArray();
            CacheItemsInBackground(itemsArray, limit, key);
            return itemsArray;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(TId masterId, TId slaveId, TManyModel item, CancellationToken token = default(CancellationToken))
        {
            await _service.UpdateAsync(masterId, slaveId, item, token);
            var id = new SlaveToMasterId<TId>(masterId, slaveId);
            await CacheMaybeSetAsync(id, _service, token);
        }

        /// <inheritdoc />
        public async Task<TManyModel> UpdateAndReturnAsync(TId masterId, TId slaveId, TManyModel item,
            CancellationToken token = default(CancellationToken))
        {
            var result = await _service.UpdateAndReturnAsync(masterId, slaveId, item, token);
            await CacheSetAsync(result, token);
            return result;
        }

        /// <inheritdoc />
        public Task DeleteAsync(TId masterId, TId slaveId, CancellationToken token = default(CancellationToken))
        {
            var task1 = _service.DeleteAsync(masterId, slaveId, token);
            var task2 = RemoveCachedChildrenInBackgroundAsync(masterId, token);
            return Task.WhenAll(task1, task2);
        }

        /// <inheritdoc />
        public Task DeleteChildrenAsync(TId masterId, CancellationToken token = default(CancellationToken))
        {
            var task1 = _service.DeleteChildrenAsync(masterId, token);
            var task2 = RemoveCachedChildrenInBackgroundAsync(masterId, token);
            return Task.WhenAll(task1, task2);
        }

        private static string CacheKeyForChildrenCollection(TId parentId)
        {
            return $"childrenOf-{parentId}";
        }

        /// <inheritdoc />
        public Task<TManyModel> ReadAsync(SlaveToMasterId<TId> id, CancellationToken token = default(CancellationToken))
        {
            return ReadAsync(id.MasterId, id.SlaveId, token);
        }
    }
}
