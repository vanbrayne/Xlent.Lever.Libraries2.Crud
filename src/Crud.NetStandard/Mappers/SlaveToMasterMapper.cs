using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.Mappers
{
    /// <inheritdoc cref="SlaveToMasterMapper{TClientModelCreate,TClientModel,TClientId,TServerModel,TServerId}" />
    public class SlaveToMasterMapper<TClientModel, TClientId, TServerModel, TServerId> :
        SlaveToMasterMapper<TClientModel, TClientModel, TClientId, TServerModel, TServerId>,
        ICrudSlaveToMaster<TClientModel, TClientId>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SlaveToMasterMapper(ICrudable service,
            ICrudMapper<TClientModel, TServerModel> mapper)
            : base(service, mapper)
        {
        }
    }

    /// <inheritdoc cref="ICrudSlaveToMaster{TModelCreate,TModel,TId}" />
    public class SlaveToMasterMapper<TClientModelCreate, TClientModel, TClientId, TServerModel, TServerId> : 
        ICrudSlaveToMaster<TClientModelCreate, TClientModel, TClientId> 
        where TClientModel : TClientModelCreate
    {
        private readonly ICrudSlaveToMaster<TServerModel, TServerId> _service;
        private readonly ICrudMapper<TClientModelCreate, TClientModel, TServerModel> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        public SlaveToMasterMapper(ICrudable service, ICrudMapper<TClientModelCreate, TClientModel, TServerModel> mapper)
        {
            _service = new SlaveToMasterPassThrough<TServerModel, TServerId>(service);
            _mapper = mapper;
        }

        /// <inheritdoc />
        public virtual async Task<TClientId> CreateAsync(TClientId masterId, TClientModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var serverMasterId = MapperHelper.MapToType<TServerId, TClientId>(masterId);
            var record = _mapper.MapToServer(item);
            var serverId = await _service.CreateAsync(serverMasterId, record, token);
            FulcrumAssert.IsNotDefaultValue(serverId);
            return MapperHelper.MapToType<TClientId, TServerId>(serverId);
        }

        /// <inheritdoc />
        public virtual async Task<TClientModel> CreateAndReturnAsync(TClientId masterId, TClientModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var serverMasterId = MapperHelper.MapToType<TServerId, TClientId>(masterId);
            var record = _mapper.MapToServer(item);
            record = await _service.CreateAndReturnAsync(serverMasterId, record, token);
            FulcrumAssert.IsNotDefaultValue(record);
            return _mapper.MapFromServer(record);
        }

        /// <inheritdoc />
        public virtual async Task CreateWithSpecifiedIdAsync(TClientId masterId, TClientId slaveId, TClientModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var serverMasterId = MapperHelper.MapToType<TServerId, TClientId>(masterId);
            var serverSlaveId = MapperHelper.MapToType<TServerId, TClientId>(slaveId);
            var record = _mapper.MapToServer(item);
            await _service.CreateWithSpecifiedIdAsync(serverMasterId, serverSlaveId, record, token);
        }

        /// <inheritdoc />
        public virtual async Task<TClientModel> CreateWithSpecifiedIdAndReturnAsync(TClientId masterId, TClientId slaveId, TClientModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var serverMasterId = MapperHelper.MapToType<TServerId, TClientId>(masterId);
            var serverSlaveId = MapperHelper.MapToType<TServerId, TClientId>(slaveId);
            var record = _mapper.MapToServer(item);
            record = await _service.CreateWithSpecifiedIdAndReturnAsync(serverMasterId, serverSlaveId, record, token);
            return _mapper.MapFromServer(record);
        }

        /// <inheritdoc />
        public virtual async Task<TClientModel> ReadAsync(TClientId masterId, TClientId slaveId, CancellationToken token = default(CancellationToken))
        {
            var serverMasterId = MapperHelper.MapToType<TServerId, TClientId>(masterId);
            var serverSlaveId = MapperHelper.MapToType<TServerId, TClientId>(slaveId);
            var record = await _service.ReadAsync(serverMasterId, serverSlaveId, token);
            return _mapper.MapFromServer(record);
        }

        /// <inheritdoc />
        public Task<TClientModel> ReadAsync(SlaveToMasterId<TClientId> id, CancellationToken token = default(CancellationToken))
        {
            return ReadAsync(id.MasterId, id.SlaveId, token);
        }

        /// <inheritdoc />
        public virtual async Task<PageEnvelope<TClientModel>> ReadChildrenWithPagingAsync(TClientId parentId, int offset, int? limit = null,
            CancellationToken token = default(CancellationToken))
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(parentId);
            var storagePage = await _service.ReadChildrenWithPagingAsync(serverId, offset, limit, token);
            FulcrumAssert.IsNotNull(storagePage?.Data);
            var data = storagePage?.Data.Select(_mapper.MapFromServer);
            return new PageEnvelope<TClientModel>(storagePage?.PageInfo, data);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TClientModel>> ReadChildrenAsync(TClientId parentId, int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(parentId);
            var items = await _service.ReadChildrenAsync(serverId, limit, token);
            FulcrumAssert.IsNotNull(items);
            return items?.Select(_mapper.MapFromServer);
        }

        /// <inheritdoc />
        public virtual Task UpdateAsync(TClientId masterId, TClientId slaveId, TClientModel item,
            CancellationToken token = default(CancellationToken))
        {
            var serverMasterId = MapperHelper.MapToType<TServerId, TClientId>(masterId);
            var serverSlaveId = MapperHelper.MapToType<TServerId, TClientId>(slaveId);
            var record = _mapper.MapToServer(item);
            return _service.UpdateAsync(serverMasterId, serverSlaveId, record, token);
        }

        /// <inheritdoc />
        public virtual async Task<TClientModel> UpdateAndReturnAsync(TClientId masterId, TClientId slaveId, TClientModel item,
            CancellationToken token = default(CancellationToken))
        {
            var serverMasterId = MapperHelper.MapToType<TServerId, TClientId>(masterId);
            var serverSlaveId = MapperHelper.MapToType<TServerId, TClientId>(slaveId);
            var record = _mapper.MapToServer(item);
            record = await _service.UpdateAndReturnAsync(serverMasterId, serverSlaveId, record, token);
            return _mapper.MapFromServer(record);
        }

        /// <inheritdoc />
        public virtual Task DeleteChildrenAsync(TClientId parentId, CancellationToken token = default(CancellationToken))
        {
            var serverId = MapperHelper.MapToType<TServerId, TClientId>(parentId);
            return _service.DeleteChildrenAsync(serverId, token);
        }

        /// <inheritdoc />
        public virtual Task DeleteAsync(TClientId masterId, TClientId slaveId, CancellationToken token = default(CancellationToken))
        {
            var serverMasterId = MapperHelper.MapToType<TServerId, TClientId>(masterId);
            var serverSlaveId = MapperHelper.MapToType<TServerId, TClientId>(slaveId);
            return _service.DeleteAsync(serverMasterId, serverSlaveId, token);
        }
    }
}
