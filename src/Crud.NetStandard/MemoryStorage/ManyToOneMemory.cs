﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Logic;
using Xlent.Lever.Libraries2.Core.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.MemoryStorage
{
    /// <summary>
    /// General class for storing a many to one item in memory.
    /// </summary>
    /// <typeparam name="TManyModel">The model for the children that each points out a parent.</typeparam>
    /// <typeparam name="TId">The type for the id field of the models.</typeparam>
    public class ManyToOneMemory<TManyModel, TId> : ManyToOneMemory<TManyModel, TManyModel, TId>, ICrud<TManyModel, TId>, IManyToOneCrud<TManyModel, TId>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getParentIdDelegate">See <see cref="ManyToOneMemory{TManyModelCreate,TManyModel,TId}.GetParentIdDelegate"/>.</param>
        public ManyToOneMemory(GetParentIdDelegate getParentIdDelegate)
        :base(getParentIdDelegate)
        {
        }
    }

    /// <summary>
    /// General class for storing a many to one item in memory.
    /// </summary>
    /// <typeparam name="TManyModel">The model for the children that each points out a parent.</typeparam>
    /// <typeparam name="TId">The type for the id field of the models.</typeparam>
    /// <typeparam name="TManyModelCreate"></typeparam>
    public class ManyToOneMemory<TManyModelCreate, TManyModel, TId> : CrudMemory<TManyModelCreate, TManyModel, TId>, IManyToOneCrud<TManyModelCreate, TManyModel, TId> 
        where TManyModel : TManyModelCreate
    {
        private readonly GetParentIdDelegate _getParentIdDelegate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getParentIdDelegate">See <see cref="GetParentIdDelegate"/>.</param>
        public ManyToOneMemory(GetParentIdDelegate getParentIdDelegate)
        {
            InternalContract.RequireNotNull(getParentIdDelegate, nameof(getParentIdDelegate));
            _getParentIdDelegate = getParentIdDelegate;
        }

        /// <summary>
        /// A delegate method for getting the parent id from a stored item.
        /// </summary>
        /// <param name="item">The item to get the parent for.</param>
        public delegate object GetParentIdDelegate(TManyModel item);

        /// <inheritdoc />
        public Task<PageEnvelope<TManyModel>> ReadChildrenWithPagingAsync(TId parentId, int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            limit = limit ?? PageInfo.DefaultLimit;
            InternalContract.RequireNotNull(parentId, nameof(parentId));
            InternalContract.RequireGreaterThanOrEqualTo(0, offset, nameof(offset));
            InternalContract.RequireGreaterThan(0, limit.Value, nameof(limit));
            lock (MemoryItems)
            {
                var list = MemoryItems.Values
                    .Where(i => parentId.Equals(_getParentIdDelegate(i)))
                    .Skip(offset)
                    .Take(limit.Value);
                var page = new PageEnvelope<TManyModel>(offset, limit.Value, MemoryItems.Count, list);
                return Task.FromResult(page);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TManyModel>> ReadChildrenAsync(TId parentId, int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotNull(parentId, nameof(parentId));
            InternalContract.RequireGreaterThan(0, limit, nameof(limit));


            var result = new List<TManyModel>();
            var offset = 0;
            while (true)
            {
                var page = await ReadChildrenWithPagingAsync(parentId, offset, null, token);
                if (page.PageInfo.Returned == 0) break;
                result.AddRange(page.Data);
                offset += page.PageInfo.Returned;
            }

            return result;
        }

        /// <inheritdoc />
        public async Task DeleteChildrenAsync(TId masterId, CancellationToken token = default(CancellationToken))
        {
            InternalContract.RequireNotNull(masterId, nameof(masterId));
            InternalContract.RequireNotDefaultValue(masterId, nameof(masterId));
            var errorMessage = $"{nameof(TManyModel)} must implement the interface {nameof(IUniquelyIdentifiable<TId>)} for this method to work.";
            InternalContract.Require(typeof(IUniquelyIdentifiable<TId>).IsAssignableFrom(typeof(TManyModel)), errorMessage);
            var items = new PageEnvelopeEnumerableAsync<TManyModel>((o, t) => ReadChildrenWithPagingAsync(masterId, o, null, t), token);
            var enumerator = items.GetEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                if (!(enumerator.Current is IUniquelyIdentifiable<TId> item)) continue;
                await DeleteAsync(item.Id, token);
            }
        }
    }
}
