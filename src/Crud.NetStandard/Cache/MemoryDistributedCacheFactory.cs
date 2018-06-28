using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Cache;
using Xlent.Lever.Libraries2.Crud.Interfaces;

namespace Xlent.Lever.Libraries2.Crud.Cache
{
    /// <summary>
    /// A factory for creating new caches.
    /// </summary>
    public class MemoryDistributedCacheFactory : IDistributedCacheFactory
    {
        private readonly ICrud<MemoryDistributedCache, MemoryDistributedCache, string> _storage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storage"></param>
        public MemoryDistributedCacheFactory(ICrud<MemoryDistributedCache, MemoryDistributedCache, string> storage)
        {
            InternalContract.RequireNotNull(storage, nameof(storage));
            _storage = storage;
        }

        /// <inheritdoc />
        public async Task<IDistributedCache> GetOrCreateDistributedCacheAsync(string key)
        {
            var cache = await _storage.ReadAsync(key);

            if (cache != null) return cache;
            cache = new MemoryDistributedCache();
            await _storage.CreateWithSpecifiedIdAsync(key, cache);
            return cache;
        }
    }
}
