using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Helpers;

namespace Xlent.Lever.Libraries2.Crud.PassThrough
{
    /// <inheritdoc cref="SlaveToMasterPassThrough{TModel,TId}" />
    public class SlaveToMasterPassThrough<TModel, TId> :
        SlaveToMasterPassThrough<TModel, TModel, TId>,
        ICrudSlaveToMaster<TModel, TId>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The crud class to pass things down to.</param>
        public SlaveToMasterPassThrough(ICrudable<TModel, TId> service)
            : base(service)
        {
        }
    }

    /// <inheritdoc cref="ICrudManyToOne{TModelCreate,TModel,TId}" />
    public class SlaveToMasterPassThrough<TModelCreate, TModel, TId> :
        ICrudSlaveToMaster<TModelCreate, TModel, TId>
         where TModel : TModelCreate
    {
        /// <summary>
        /// The service to pass the calls to.
        /// </summary>
        protected readonly ICrudable<TModel, TId> Service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The crud class to pass things down to.</param>
        public SlaveToMasterPassThrough(ICrudable<TModel, TId> service)
        {
            InternalContract.RequireNotNull(service, nameof(service));
            Service = service;
        }

        /// <inheritdoc />
        public virtual Task<TId> CreateAsync(TId masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<ICreateSlave<TModelCreate, TModel, TId>>(Service);
            return implementation.CreateAsync(masterId, item, token);
        }

        /// <inheritdoc />
        public virtual Task<TModel> CreateAndReturnAsync(TId masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<ICreateSlaveAndReturn<TModelCreate, TModel, TId>>(Service);
            return implementation.CreateAndReturnAsync(masterId, item, token);
        }

        /// <inheritdoc />
        public virtual Task CreateWithSpecifiedIdAsync(TId masterId, TId slaveId, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<ICreateSlaveWithSpecifiedId<TModelCreate, TModel, TId>>(Service);
            return implementation.CreateWithSpecifiedIdAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public virtual Task<TModel> CreateWithSpecifiedIdAndReturnAsync(TId masterId, TId slaveId, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<ICreateSlaveWithSpecifiedId<TModelCreate, TModel, TId>>(Service);
            return implementation.CreateWithSpecifiedIdAndReturnAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public virtual Task<TModel> ReadAsync(TId masterId, TId slaveId, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IReadSlave<TModel, TId>>(Service);
            return implementation.ReadAsync(masterId, slaveId, token);
        }

        /// <inheritdoc />
        public Task<TModel> ReadAsync(SlaveToMasterId<TId> id, CancellationToken token = default(CancellationToken))
        {
            return ReadAsync(id.MasterId, id.SlaveId, token);
        }

        /// <inheritdoc />
        public virtual Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(TId parentId, int offset, int? limit = null,
            CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IReadChildrenWithPaging<TModel, TId>>(Service);
            return implementation.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
        }

        /// <inheritdoc />
        public virtual Task<IEnumerable<TModel>> ReadChildrenAsync(TId parentId, int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IReadChildren<TModel, TId>>(Service);
            return implementation.ReadChildrenAsync(parentId, limit, token);
        }

        /// <inheritdoc />
        public virtual Task UpdateAsync(TId masterId, TId slaveId, TModel item, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IUpdateSlave<TModel, TId>>(Service);
            return implementation.UpdateAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public virtual Task<TModel> UpdateAndReturnAsync(TId masterId, TId slaveId, TModel item, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IUpdateSlaveAndReturn<TModel, TId>>(Service);
            return implementation.UpdateAndReturnAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public virtual Task DeleteAsync(TId masterId, TId slaveId, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IDeleteSlave<TId>>(Service);
            return implementation.DeleteAsync(masterId, slaveId, token);
        }

        /// <inheritdoc />
        public virtual Task DeleteChildrenAsync(TId parentId, CancellationToken token = default(CancellationToken))
        {
            var implementation = CrudHelper.GetImplementationOrThrow<IDeleteChildren<TId>>(Service);
            return implementation.DeleteChildrenAsync(parentId, token);
        }
    }
}
