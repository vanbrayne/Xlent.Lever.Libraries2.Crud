﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Error.Logic;
using Xlent.Lever.Libraries2.Core.Storage.Logic;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Mappers;
using Xlent.Lever.Libraries2.Crud.Model;

namespace Xlent.Lever.Libraries2.Crud.MemoryStorage
{
    /// <summary>
    /// General class for storing any <see cref="IUniquelyIdentifiable{TId}"/> in memory.
    /// </summary>
    /// <typeparam name="TModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId"></typeparam>
    public class CrudMemory<TModel, TId> : 
        CrudMemory<TModel, TModel, TId>, 
        ICrud<TModel, TId>
    {
    }

    /// <summary>
    /// General class for storing any <see cref="IUniquelyIdentifiable{TId}"/> in memory.
    /// </summary>
    /// <typeparam name="TModelCreate">The type for creating objects in persistant storage.</typeparam>
    /// <typeparam name="TModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId"></typeparam>
    public class CrudMemory<TModelCreate, TModel, TId> :
        MemoryBase<TModel, TId>, 
        ICrud<TModelCreate, TModel, TId>
        where TModel : TModelCreate
    {
        /// <inheritdoc />
        public async Task<TId> CreateAsync(TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotNull(item, nameof(item));
            InternalContract.RequireValidated(item, nameof(item));
            var id = StorageHelper.CreateNewId<TId>();
            await CreateWithSpecifiedIdAsync(id, item, token);
            return id;
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotNull(item, nameof(item));
            InternalContract.RequireValidated(item, nameof(item));
            var id = await CreateAsync(item, token);
            return await ReadAsync(id, token);
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(TId id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            InternalContract.RequireNotNull(item, nameof(item));
            InternalContract.RequireValidated(item, nameof(item));

            var itemCopy = CopyItem(item);

            StorageHelper.MaybeCreateNewEtag(itemCopy);
            StorageHelper.MaybeUpdateTimeStamps(itemCopy, true);
            lock (MemoryItems)
            {
                ValidateNotExists(id);
                StorageHelper.MaybeSetId(id, itemCopy);
                var success = MemoryItems.TryAdd(id, itemCopy);
                if (!success) throw new FulcrumConflictException($"Item with id {id} already exists.");
            }
            await Task.Yield();
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(TId id, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotNull(item, nameof(item));
            InternalContract.RequireValidated(item, nameof(item));
            await CreateWithSpecifiedIdAsync(id, item, token);
            return await ReadAsync(id, token);
        }

        /// <inheritdoc />
        public Task<TModel> ReadAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));

            var itemCopy = GetMemoryItem(id, true);
            return Task.FromResult(itemCopy);
        }

        /// <inheritdoc />
        public Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            limit = limit ?? PageInfo.DefaultLimit;
            InternalContract.RequireGreaterThanOrEqualTo(0, offset, nameof(offset));
            InternalContract.RequireGreaterThan(0, limit.Value, nameof(limit));
            lock (MemoryItems)
            {
                var keys = MemoryItems.Keys.Skip(offset).Take(limit.Value);
                var list = keys
                    .Select(id => GetMemoryItem(id, true))
                    .Where(item => item != null)
                    .ToList();
                var page = new PageEnvelope<TModel>(offset, limit.Value, MemoryItems.Count, list);
                return Task.FromResult(page);
            }
        }

        /// <inheritdoc />
        public Task<IEnumerable<TModel>> ReadAllAsync(int limit = Int32.MaxValue,
            CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireGreaterThan(0, limit, nameof(limit));
            lock (MemoryItems)
            {
                var keys = MemoryItems.Keys.Take(limit);
                var list = keys
                    .Select(id => GetMemoryItem(id, true))
                    .Where(item => item != null)
                    .ToList();
                return Task.FromResult((IEnumerable<TModel>)list);
            }
        }

        /// <inheritdoc />
        public async Task UpdateAsync(TId id, TModel item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            InternalContract.RequireNotNull(item, nameof(item));
            InternalContract.RequireValidated(item, nameof(item));
            if (!Exists(id)) throw new FulcrumNotFoundException($"Update failed. Could not find an item with id {id}.");

            var oldValue = await MaybeVerifyEtagForUpdateAsync(id, item, this, token);
            var itemCopy = CopyItem(item);
            StorageHelper.MaybeUpdateTimeStamps(itemCopy, false);
            StorageHelper.MaybeCreateNewEtag(itemCopy);

            MemoryItems[id] = itemCopy;
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(TId id, TModel item, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));
            InternalContract.RequireNotNull(item, nameof(item));
            InternalContract.RequireValidated(item, nameof(item));
            await UpdateAsync(id, item, token);
            return await ReadAsync(id, token);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Idempotent, i.e. will not throw an exception if the item does not exist.
        /// </remarks>
        public Task DeleteAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));

            MemoryItems.TryRemove(id, out var _);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task DeleteAllAsync(CancellationToken token = default(CancellationToken))
        {
            lock (MemoryItems)
            {
                MemoryItems.Clear();
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<Lock> ClaimLockAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotDefaultValue(id, nameof(id));

            var key = MapperHelper.MapToType<string, TId>(id);
            var newLock = new Lock
            {
                ItemId = key,
                LockId = Guid.NewGuid().ToString(),
                ValidUntil = DateTimeOffset.Now.AddSeconds(30)
            };
            while (true)
            {
                token.ThrowIfCancellationRequested();
                if (_locks.TryAdd(key, newLock)) return Task.FromResult(newLock);
                if (!_locks.TryGetValue(key, out var oldLock)) continue;
                var remainingTime = oldLock.ValidUntil.Subtract(DateTimeOffset.Now);
                if (remainingTime > TimeSpan.Zero)
                {
                    var message = $"Item {key} is locked by someone else. The lock will be released before {oldLock.ValidUntil}";
                    var exception = new FulcrumTryAgainException(message)
                    {
                        RecommendedWaitTimeInSeconds = remainingTime.Seconds
                    };
                    throw exception;
                }
                if (_locks.TryUpdate(key, newLock, oldLock)) return Task.FromResult(newLock);
            }
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(Lock @lock, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotNull(@lock, nameof(@lock));
            InternalContract.RequireValidated(@lock, nameof(@lock));
            var key = @lock.ItemId;
            // Try to temporarily add additional time to make sure that nobody steals the lock while we are releasing it.
            // The TryUpdate will return false if there is no lock or if the current lock differs from the lock we want to release.
            @lock.ValidUntil = DateTimeOffset.Now.AddSeconds(30);
            if (!_locks.TryUpdate(key, @lock, @lock)) return Task.CompletedTask;
            // Finally remove the lock
            _locks.TryRemove(key, out var currentLock);
            return Task.CompletedTask;
        }

        #region private

        private void ValidateNotExists(TId id)
        {
            if (!Exists(id)) return;
            throw new FulcrumConflictException(
                $"An item of type {typeof(TModel).Name} with id \"{id}\" already exists.");
        }

        private static TModel CopyItem(TModelCreate source)
        {
            InternalContract.RequireNotNull(source, nameof(source));
            var itemCopy = StorageHelper.DeepCopy<TModel, TModelCreate>(source);
            if (itemCopy == null)
                throw new FulcrumAssertionFailedException("Could not copy an item.");
            return itemCopy;
        }
        #endregion
    }
}

