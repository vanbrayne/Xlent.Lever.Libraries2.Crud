using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Assert;
using Xlent.Lever.Libraries2.Core.Crud.Model;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.ServerTranslators.From
{
    /// <summary>
    /// Decorate values from the server into concept values.
    /// </summary>
    public class SlaveToMasterFromServerTranslator<TModel> : SlaveToMasterFromServerTranslator<TModel, TModel>,
        ICrudSlaveToMaster<TModel, string>
    {
        /// <inheritdoc />
        public SlaveToMasterFromServerTranslator(ICrudable storage,
            string masterIdConceptName, string slaveIdConceptName, System.Func<string> getServerNameMethod)
            : base(storage, masterIdConceptName, slaveIdConceptName, getServerNameMethod)
        {
        }
    }

    /// <summary>
    /// Decorate values from the server into concept values.
    /// </summary>
    public class SlaveToMasterFromServerTranslator<TModelCreate, TModel> : ServerTranslatorBase, ICrudSlaveToMaster<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrudSlaveToMaster<TModelCreate, TModel, string> _storage;
        private readonly string _masterIdConceptName;

        /// <inheritdoc />
        public SlaveToMasterFromServerTranslator(ICrudable storage, string masterIdConceptName, string slaveIdConceptName, System.Func<string> getServerNameMethod)
            : base(slaveIdConceptName, getServerNameMethod)
        {
            _storage = new SlaveToMasterPassThrough<TModelCreate, TModel, string>(storage);
            _masterIdConceptName = masterIdConceptName;
        }

        /// <inheritdoc />
        public async Task<SlaveToMasterId<string>> CreateAsync(string masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var result = await _storage.CreateAsync(masterId, item, token);
            var translator = CreateTranslator();
            return translator.Decorate(_masterIdConceptName, IdConceptName, result);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(string masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var result = await _storage.CreateAndReturnAsync(masterId, item, token);
            var translator = CreateTranslator();
            FulcrumAssert.IsNotNull(result);
            return translator.DecorateItem(result);
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(string masterId, string slaveId, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            await _storage.CreateWithSpecifiedIdAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string masterId, string slaveId, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var result = await _storage.CreateWithSpecifiedIdAndReturnAsync(masterId, slaveId, item, token);
            var translator = CreateTranslator();
            FulcrumAssert.IsNotNull(result);
            return translator.DecorateItem(result);
        }

        /// <inheritdoc />
        public async Task<TModel> ReadAsync(string masterId, string slaveId, CancellationToken token = default(CancellationToken))
        {
            var result = await _storage.ReadAsync(masterId, slaveId, token);
            var translator = CreateTranslator();
            FulcrumAssert.IsNotNull(result);
            return translator.DecorateItem(result);
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(string parentId, int offset, int? limit = null,
            CancellationToken token = new CancellationToken())
        {
            var result = await _storage.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(result);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> ReadChildrenAsync(string parentId, int limit = int.MaxValue, CancellationToken token = new CancellationToken())
        {
            var result = await _storage.ReadChildrenAsync(parentId, limit, token);
            var translator = CreateTranslator();
            return translator.DecorateItems(result);
        }

        /// <inheritdoc />
        public Task UpdateAsync(string masterId, string slaveId, TModel item, CancellationToken token = default(CancellationToken))
        {
            return _storage.UpdateAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(string masterId, string slaveId, TModel item,
            CancellationToken token = default(CancellationToken))
        {
            var result = await _storage.UpdateAndReturnAsync(masterId, slaveId, item, token);
            var translator = CreateTranslator();
            FulcrumAssert.IsNotNull(result);
            return translator.DecorateItem(result);
        }

        /// <inheritdoc />
        public Task DeleteChildrenAsync(string masterId, CancellationToken token = default(CancellationToken))
        {
            return _storage.DeleteChildrenAsync(masterId, token);
        }

        /// <inheritdoc />
        public Task DeleteAsync(string masterId, string slaveId, CancellationToken token = default(CancellationToken))
        {
            return _storage.DeleteAsync(masterId, slaveId, token);
        }
    }
}