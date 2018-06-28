﻿using System;
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

    /// <inheritdoc cref="ManyToOneAutoCache{TManyModelCreate,TManyModel,TId}" />
    public class ManyToOneAutoCache<TManyModel, TId> : 
        ManyToOneAutoCache<TManyModel, TManyModel, TId>,
        ICrud<TManyModel, TId>, ICrudManyToOne<TManyModel, TId>
    {
        /// <summary>
        /// Constructor for TOneModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public ManyToOneAutoCache(ICrudManyToOne<TManyModel, TId> service, IDistributedCache cache,
            FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
            : this(service, item => ((IUniquelyIdentifiable<TId>)item).Id, cache, flushCacheDelegateAsync, options)
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
        public ManyToOneAutoCache(ICrudManyToOne<TManyModel, TId> service,
            GetIdDelegate<TManyModel, TId> getIdDelegate, IDistributedCache cache,
            FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
            : base(service, getIdDelegate, cache, flushCacheDelegateAsync, options)
        {
        }
    }

    /// <inheritdoc cref="CrudAutoCache{TModel,TId}" />
    public class ManyToOneAutoCache<TManyModelCreate, TManyModel, TId> :
        CrudAutoCache<TManyModelCreate, TManyModel, TId>,
        ICrudManyToOne<TManyModelCreate, TManyModel, TId> where TManyModel : TManyModelCreate
    {
        private readonly ICrudManyToOne<TManyModelCreate, TManyModel, TId> _service;

        /// <summary>
        /// Constructor for TOneModel that implements <see cref="IUniquelyIdentifiable{TId}"/>.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="cache"></param>
        /// <param name="flushCacheDelegateAsync"></param>
        /// <param name="options"></param>
        public ManyToOneAutoCache(ICrudManyToOne<TManyModelCreate, TManyModel, TId> service,
            IDistributedCache cache, FlushCacheDelegateAsync flushCacheDelegateAsync = null,
            AutoCacheOptions options = null)
            : this(service, item => ((IUniquelyIdentifiable<TId>)item).Id, cache, flushCacheDelegateAsync, options)
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
        public ManyToOneAutoCache(ICrudManyToOne<TManyModelCreate, TManyModel, TId> service,
            GetIdDelegate<TManyModel, TId> getIdDelegate, IDistributedCache cache,
            FlushCacheDelegateAsync flushCacheDelegateAsync = null, AutoCacheOptions options = null)
            : base(service, getIdDelegate, cache, flushCacheDelegateAsync, options)
        {
            InternalContract.RequireNotNull(service, nameof(service));
            InternalContract.RequireNotNull(getIdDelegate, nameof(getIdDelegate));
            InternalContract.RequireNotNull(cache, nameof(cache));
            _service = service;
        }

        /// <summary>
        /// True while a background thread is active saving results from a ReadAll() operation.
        /// </summary>
        protected bool IsCollectionOperationActive(TId parentId)
        {
            InternalContract.RequireNotDefaultValue(parentId, nameof(parentId));
            return IsCollectionOperationActive(CacheKeyForChildrenCollection(parentId));
        }

        /// <summary>
        /// Wait until any background thread is active saving results from a ReadAll() operation.
        /// </summary>
        public async Task DelayUntilNoOperationActiveAsync(TId parentId)
        {
            var count = 0;
            while (count++ < 5 && !IsCollectionOperationActive(parentId)) await Task.Delay(TimeSpan.FromMilliseconds(1));
            while (IsCollectionOperationActive(parentId)) await Task.Delay(TimeSpan.FromMilliseconds(10));
        }

        /// <inheritdoc />
        public async Task DeleteChildrenAsync(TId masterId, CancellationToken token = default(CancellationToken))
        {
            await _service.DeleteChildrenAsync(masterId, token);
            await RemoveCachedChildrenInBackgroundAsync(masterId, token);
        }

        private async Task RemoveCachedChildrenInBackgroundAsync(TId parentId, CancellationToken token = default(CancellationToken))
        {
            var key = CacheKeyForChildrenCollection(parentId);
            await RemoveCacheItemsInBackgroundAsync(key, async () => await CacheGetAsync(int.MaxValue, key, token));
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
        public async Task<IEnumerable<TManyModel>> ReadChildrenAsync(TId parentId, int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(parentId, nameof(parentId));
            InternalContract.RequireGreaterThan(0, limit, nameof(limit));
            var key = CacheKeyForChildrenCollection(parentId);
            var itemsArray = await CacheGetAsync(limit, key, token);
            if (itemsArray != null) return itemsArray;
            var itemsCollection = await _service.ReadChildrenAsync(parentId, limit, token);
            itemsArray = itemsCollection as TManyModel[] ?? itemsCollection.ToArray();
            CacheItemsInBackground(itemsArray, limit, key);
            return itemsArray;
        }

        private static string CacheKeyForChildrenCollection(TId parentId)
        {
            return $"childrenOf-{parentId}";
        }
    }
}
