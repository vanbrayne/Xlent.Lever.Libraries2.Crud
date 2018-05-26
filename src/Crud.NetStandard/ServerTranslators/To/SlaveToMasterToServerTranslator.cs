using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Core.Translation;

namespace Xlent.Lever.Libraries2.Crud.ServerTranslators.To
{
    /// <inheritdoc cref="SlaveToMasterToServerTranslator{TModelCreate, TModel}" />
    public class SlaveToMasterToServerTranslator<TModel> :
        SlaveToMasterToServerTranslator<TModel, TModel>,
        ICrudSlaveToMaster<TModel, string>
    {
        /// <inheritdoc />
        public SlaveToMasterToServerTranslator(ICrudSlaveToMaster<TModel, string> storage,
            System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(null, getServerNameMethod, translatorService)
        {
        }
    }

    /// <inheritdoc cref="ServerTranslatorBase" />
    public class SlaveToMasterToServerTranslator<TModelCreate, TModel> : 
        ServerTranslatorBase, 
        ICrudSlaveToMaster<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrudSlaveToMaster<TModelCreate, TModel, string> _storage;

        /// <inheritdoc />
        public SlaveToMasterToServerTranslator(ICrudSlaveToMaster<TModelCreate, TModel, string> storage, System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(null, getServerNameMethod, translatorService)
        {
            _storage = storage;
        }

        /// <inheritdoc />
        public async Task<SlaveToMasterId<string>> CreateAsync(string masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            item = translator.Translate(item);
            return await _storage.CreateAsync(masterId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(string masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            item = translator.Translate(item);
            return await _storage.CreateAndReturnAsync(masterId, item, token);
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(string masterId, string slaveId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            item = translator.Translate(item);
            await _storage.CreateWithSpecifiedIdAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string masterId, string slaveId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            item = translator.Translate(item);
            return await _storage.CreateWithSpecifiedIdAndReturnAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> ReadAsync(string masterId, string slaveId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            return await _storage.ReadAsync(masterId, slaveId, token);
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(string parentId, int offset, int? limit = null,
        CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(parentId).ExecuteAsync();
            parentId = translator.Translate(parentId);
            return await _storage.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> ReadChildrenAsync(string parentId, int limit = int.MaxValue, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(parentId).ExecuteAsync();
            parentId = translator.Translate(parentId);
            return await _storage.ReadChildrenAsync(parentId, limit, token);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(string masterId, string slaveId, TModel item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            item = translator.Translate(item);
            await _storage.UpdateAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(string masterId, string slaveId, TModel item,
            CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            item = translator.Translate(item);
            return await _storage.UpdateAndReturnAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task DeleteChildrenAsync(string masterId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).ExecuteAsync();
            masterId = translator.Translate(masterId);
            await _storage.DeleteChildrenAsync(masterId, token);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(string masterId, string slaveId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            await _storage.DeleteAsync(masterId, slaveId, token);
        }
    }
}