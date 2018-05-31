﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Helpers;

namespace Xlent.Lever.Libraries2.Crud.PassThrough
{
    /// <inheritdoc cref="ManyToOnePassThrough{TModelCreate,TModel,TId}" />
    public class ManyToOnePassThrough<TModel, TId> : 
        ManyToOnePassThrough<TModel, TModel, TId>,
        ICrudManyToOne<TModel, TId>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The crud class to pass things down to.</param>
        public ManyToOnePassThrough(ICrudable<TModel, TId> service)
            : base(service)
        {
        }
    }

    /// <inheritdoc cref="ICrudManyToOne{TModelCreate,TModel,TId}" />
    public class ManyToOnePassThrough<TModelCreate, TModel, TId> : CrudPassThrough<TModelCreate, TModel, TId>, ICrudManyToOne<TModelCreate, TModel, TId> where TModel : TModelCreate
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The crud class to pass things down to.</param>
        public ManyToOnePassThrough(ICrudable<TModel, TId> service)
            : base(service)
        {
        }

        /// <inheritdoc />
        public virtual Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(TId parentId, int offset, int? limit = null,
            CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IReadChildrenWithPaging<TModel, TId>>(Service);
            return implementation.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TModel>> ReadChildrenAsync(TId parentId, int limit = Int32.MaxValue, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IReadChildren<TModel, TId>>(Service);
            return implementation.ReadChildrenAsync(parentId, limit, token);
        }

        /// <inheritdoc />
        public virtual Task DeleteChildrenAsync(TId parentId, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IDeleteChildren<TId>>(Service);
            return implementation.DeleteChildrenAsync(parentId, token);
        }
    }
}
