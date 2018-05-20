﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Error.Logic;
using Xlent.Lever.Libraries2.Crud.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Crud.Helpers
{
    /// <summary>
    /// Abstract base class that has a default implementation for <see cref="ReadChildrenAsync"/>.
    /// </summary>
    public abstract class SlaveToMasterBase<TModel, TId> :
        ISlaveToMaster<TModel, TId>
    {
        /// <inheritdoc />
        public abstract Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(TId parentId, int offset, int? limit = null,
            CancellationToken token = default(CancellationToken));

        /// <inheritdoc />
        public Task<IEnumerable<TModel>> ReadChildrenAsync(TId parentId, int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            return StorageHelper.ReadPagesAsync((offset, ct) => ReadChildrenWithPagingAsync(parentId, offset, null, ct), limit, token);
        }
    }
}
