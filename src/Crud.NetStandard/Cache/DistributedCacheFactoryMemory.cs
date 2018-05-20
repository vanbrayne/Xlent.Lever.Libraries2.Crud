﻿using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Error.Logic;

namespace Xlent.Lever.Libraries2.Crud.Cache
{
    /// <summary>
    /// A factory for creating new caches.
    /// </summary>
    public class DistributedCacheFactoryMemory : IDistributedCacheFactory
    {
        private readonly ICrd<DistributedCacheMemory, DistributedCacheMemory, string> _storage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storage"></param>
        public DistributedCacheFactoryMemory(ICrd<DistributedCacheMemory, DistributedCacheMemory, string> storage)
        {
            _storage = storage;
        }

        /// <inheritdoc />
        public async Task<IDistributedCache> GetOrCreateDistributedCacheAsync(string key)
        {
            var item = await _storage.ReadAsync(key);
            if (item != null) return item;
            var cache = new DistributedCacheMemory();
            await _storage.CreateWithSpecifiedIdAsync(key, cache);
            return cache;
        }
    }
}
