﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xlent.Lever.Libraries2.Core.Application;
using Xlent.Lever.Libraries2.Crud.Cache;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.MemoryStorage;

namespace Xlent.Lever.Libraries2.Crud.NetFramework.Test.Crud.Cache
{
    [TestClass]
    public class TestAutoCacheCrud : TestAutoCacheBase<string, string>
    {
        private CrudAutoCache<string, string, Guid> _autoCache;

        /// <inheritdoc />
        public override CrudAutoCache<string, Guid> CrudAutoCache => _autoCache;

        private ICrud<string, string, Guid> _storage;
        /// <inheritdoc />
        protected override ICrud<string, string, Guid> CrudStorage => _storage;

        [TestInitialize]
        public void Initialize()
        {
            FulcrumApplicationHelper.UnitTestSetup(typeof(TestAutoCacheCrud).FullName);
            _storage = new CrudMemory<string, string, Guid>();
            Cache = new MemoryDistributedCache();
            DistributedCacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1000)
            };
            AutoCacheOptions = new AutoCacheOptions
            {
                AbsoluteExpirationRelativeToNow = DistributedCacheOptions.AbsoluteExpirationRelativeToNow
            };
            _autoCache = new CrudAutoCache<string, string, Guid>(_storage, ToGuid, Cache, null, AutoCacheOptions);
        }

        [TestMethod]
        public async Task UpdateStorage_ReadNewCache()
        {
            var id = Guid.NewGuid();
            await PrepareStorageAndCacheAsync(id, "A", "A");
            await _autoCache.UpdateAndReturnAsync(id, "B");
            await PrepareStorageAsync(id, "C");
            await VerifyAsync(id, "C", "B");
        }

        [TestMethod]
        public async Task UpdateStorage_ReadOldCache()
        {
            var id = Guid.NewGuid();
            await PrepareStorageAndCacheAsync(id, "A", "A");
            await _autoCache.UpdateAsync(id, "B"); // Will not update cache
            await VerifyAsync(id, "B", "A");
        }

        [TestMethod]
        public async Task UpdateStorage_GetOption_ReadCache()
        {
            AutoCacheOptions.DoGetToUpdate = true;
            _autoCache = new CrudAutoCache<string, string, Guid>(_storage, ToGuid, Cache, null, AutoCacheOptions);
            var id = Guid.NewGuid();
            await PrepareStorageAndCacheAsync(id, "A", "A");
            await _autoCache.UpdateAsync(id, "B"); // Will update cache thanks to DoGetToUpdate
            await PrepareStorageAsync(id, "C");
            await VerifyAsync(id, "C", "B");
        }

        [TestMethod]
        public async Task UpdateStorage_SaveOption_ReadCache()
        {
            AutoCacheOptions.SaveAll = true;
            _autoCache = new CrudAutoCache<string, string, Guid>(_storage, ToGuid, Cache, null, AutoCacheOptions);
            var id = Guid.NewGuid();
            await PrepareStorageAndCacheAsync(id, "A", "A");
            await _autoCache.UpdateAsync(id, "B"); // Will update cache thanks to SaveAll
            await PrepareStorageAsync(id, "C");
            await VerifyAsync(id, "C", "B");
        }
    }
}
