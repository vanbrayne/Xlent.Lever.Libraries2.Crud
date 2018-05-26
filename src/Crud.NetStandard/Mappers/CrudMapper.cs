using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Model;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.Mappers
{
    /// <inheritdoc cref="CrudMapper{TClientModelCreate,TClientModel,TClientId,TServerModel,TServerId}" />
    public class
        CrudMapper<TClientModel, TClientId, TServerModel, TServerId> : CrudMapper<TClientModel, TClientModel, TClientId, TServerModel, TServerId>, ICrud<TClientModel, TClientId>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CrudMapper(ICrudable service,
            ICrudMapper<TClientModel, TServerModel> mapper)
        :base(service, mapper)
        {
        }
    }

    /// <inheritdoc cref="ICrud{TModel,TId}" />
    public class CrudMapper<TClientModelCreate, TClientModel, TClientId, TServerModel, TServerId> : ICrud<TClientModelCreate, TClientModel, TClientId>
        where TClientModel : TClientModelCreate
    {
        private readonly ICrud<TServerModel, TServerId> _service;
        private readonly ICrudMapper<TClientModelCreate, TClientModel, TServerModel> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        public CrudMapper(ICrudable service, ICrudMapper<TClientModelCreate, TClientModel, TServerModel> mapper)
        {
            _service = new CrudPassThrough<TServerModel, TServerId>(service);
            _mapper = mapper;
        }

        /// <inheritdoc />
        public virtual async Task<TClientId> CreateAsync(TClientModelCreate item, CancellationToken token = new CancellationToken())
        {
            var record = _mapper.MapToServer(item);
            var serverId = await _service.CreateAsync(record, token);
            FulcrumAssert.IsNotDefaultValue(serverId);
            return MapperHelper.MapToType<TClientId, TServerId>(serverId);
        }

        /// <inheritdoc />
        public virtual async Task<TClientModel> CreateAndReturnAsync(TClientModelCreate item, CancellationToken token = new CancellationToken())
        {
            var record = _mapper.MapToServer(item);
            record = await _service.CreateAndReturnAsync(record, token);
            FulcrumAssert.IsNotDefaultValue(record);
            return _mapper.MapFromServer(record);
        }

        /// <inheritdoc />
        public virtual Task CreateWithSpecifiedIdAsync(TClientId id, TClientModelCreate item, CancellationToken token = new CancellationToken())
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(id);
            var record = _mapper.MapToServer(item);
            return _service.CreateWithSpecifiedIdAsync(serverId, record, token);
        }

        /// <inheritdoc />
        public virtual async Task<TClientModel> CreateWithSpecifiedIdAndReturnAsync(TClientId id, TClientModelCreate item,
            CancellationToken token = new CancellationToken())
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(id);
            var record = _mapper.MapToServer(item);
            record = await _service.CreateWithSpecifiedIdAndReturnAsync(serverId, record, token);
            return _mapper.MapFromServer(record);
        }

        /// <inheritdoc />
        public virtual async Task<TClientModel> ReadAsync(TClientId id, CancellationToken token = new CancellationToken())
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(id);
            var record = await _service.ReadAsync(serverId, token);
            return _mapper.MapFromServer(record);
        }

        /// <inheritdoc />
        public virtual async Task<PageEnvelope<TClientModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = new CancellationToken())
        {
            var storagePage = await _service.ReadAllWithPagingAsync(offset, limit, token);
            FulcrumAssert.IsNotNull(storagePage?.Data);
            var data = storagePage?.Data.Select(_mapper.MapFromServer);
            return new PageEnvelope<TClientModel>(storagePage?.PageInfo, data);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TClientModel>> ReadAllAsync(int limit = 2147483647, CancellationToken token = new CancellationToken())
        {
            var items = await _service.ReadAllAsync(limit, token);
            FulcrumAssert.IsNotNull(items);
            return items?.Select(_mapper.MapFromServer);
        }

        /// <inheritdoc />
        public virtual Task UpdateAsync(TClientId id, TClientModel item, CancellationToken token = new CancellationToken())
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(id);
            var record = _mapper.MapToServer(item);
            return _service.UpdateAsync(serverId, record, token);
        }

        /// <inheritdoc />
        public virtual async Task<TClientModel> UpdateAndReturnAsync(TClientId id, TClientModel item, CancellationToken token = new CancellationToken())
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(id);
            var record = _mapper.MapToServer(item);
            record = await _service.UpdateAndReturnAsync(serverId, record, token);
            return _mapper.MapFromServer(record);
        }

        /// <inheritdoc />
        public virtual Task DeleteAsync(TClientId id, CancellationToken token = new CancellationToken())
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(id);
            return _service.DeleteAsync(serverId, token);
        }

        /// <inheritdoc />
        public virtual Task DeleteAllAsync(CancellationToken token = new CancellationToken())
        {
            return _service.DeleteAllAsync(token);
        }

        /// <inheritdoc />
        public Task<Lock> ClaimLockAsync(TClientId id, CancellationToken token = new CancellationToken())
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(id);
            return _service.ClaimLockAsync(serverId, token);
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(Lock @lock, CancellationToken token = new CancellationToken())
        {
            return _service.ReleaseLockAsync(@lock, token);
        }
    }
}