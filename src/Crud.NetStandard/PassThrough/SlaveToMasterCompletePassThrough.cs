﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Crud.PassThrough
{
    /// <inheritdoc cref="SlaveToMasterCrudPassThrough{TManyModelCreate,TManyModel,TId}" />
    public class SlaveToMasterCrudPassThrough<TModel, TId> :
        SlaveToMasterCrudPassThrough<TModel, TModel, TId>,
        ISlaveToMaster<TModel, TId>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nextLevel">The crud class to pass things down to.</param>
        public SlaveToMasterCrudPassThrough(ISlaveToMasterCrud<TModel, TId> nextLevel)
            : base(nextLevel)
        {
        }
    }

    /// <inheritdoc cref="IManyToOneCrud{TManyModelCreate,TManyModel,TId}" />
    public class SlaveToMasterCrudPassThrough<TModelCreate, TModel, TId> :
        RudPassThrough<TModel, SlaveToMasterId<TId>>,
        ISlaveToMasterCrud<TModelCreate, TModel, TId>
         where TModel : TModelCreate
    {
        private readonly ISlaveToMasterCrud<TModelCreate, TModel, TId> _nextLevel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nextLevel">The crud class to pass things down to.</param>
        public SlaveToMasterCrudPassThrough(ISlaveToMasterCrud<TModelCreate, TModel, TId> nextLevel)
        :base(nextLevel)
        {
            _nextLevel = nextLevel;
        }

        /// <inheritdoc />
        public virtual Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(TId parentId, int offset, int? limit = null,
            CancellationToken token = default(CancellationToken))
        {
            return _nextLevel.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TModel>> ReadChildrenAsync(TId parentId, int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            return _nextLevel.ReadChildrenAsync(parentId, limit, token);
        }

        /// <inheritdoc />
        public virtual Task<SlaveToMasterId<TId>> CreateAsync(TId masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            return _nextLevel.CreateAsync(masterId, item, token);
        }

        /// <inheritdoc />
        public virtual Task<TModel> CreateAndReturnAsync(TId masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            return _nextLevel.CreateAndReturnAsync(masterId, item, token);
        }

        /// <inheritdoc />
        public virtual Task CreateWithSpecifiedIdAsync(SlaveToMasterId<TId> id, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            return _nextLevel.CreateWithSpecifiedIdAsync(id, item, token);
        }

        /// <inheritdoc />
        public virtual Task<TModel> CreateWithSpecifiedIdAndReturnAsync(SlaveToMasterId<TId> id, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            return _nextLevel.CreateWithSpecifiedIdAndReturnAsync(id, item, token);
        }

        /// <inheritdoc />
        public virtual Task DeleteChildrenAsync(TId parentId, CancellationToken token = default(CancellationToken))
        {
            return _nextLevel.DeleteChildrenAsync(parentId, token);
        }
    }
}