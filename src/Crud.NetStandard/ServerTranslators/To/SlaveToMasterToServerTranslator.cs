﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Core.Translation;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.ServerTranslators.To
{
    /// <inheritdoc cref="SlaveToMasterToServerTranslator{TModelCreate, TModel}" />
    public class SlaveToMasterToServerTranslator<TModel> :
        SlaveToMasterToServerTranslator<TModel, TModel>,
        ICrudSlaveToMaster<TModel, string>
    {
        /// <inheritdoc />
        public SlaveToMasterToServerTranslator(ICrudable service,
            System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(service, getServerNameMethod, translatorService)
        {
        }
    }

    /// <inheritdoc cref="ServerTranslatorBase" />
    public class SlaveToMasterToServerTranslator<TModelCreate, TModel> : 
        ServerTranslatorBase, 
        ICrudSlaveToMaster<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrudSlaveToMaster<TModelCreate, TModel, string> _service;

        /// <inheritdoc />
        public SlaveToMasterToServerTranslator(ICrudable service, System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(null, getServerNameMethod, translatorService)
        {
            _service = new SlaveToMasterPassThrough<TModelCreate, TModel, string>(service);
        }

        /// <inheritdoc />
        public async Task<string> CreateAsync(string masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            item = translator.Translate(item);
            return await _service.CreateAsync(masterId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(string masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            item = translator.Translate(item);
            return await _service.CreateAndReturnAsync(masterId, item, token);
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(string masterId, string slaveId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            item = translator.Translate(item);
            await _service.CreateWithSpecifiedIdAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string masterId, string slaveId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            item = translator.Translate(item);
            return await _service.CreateWithSpecifiedIdAndReturnAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> ReadAsync(string masterId, string slaveId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            return await _service.ReadAsync(masterId, slaveId, token);
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(string parentId, int offset, int? limit = null,
        CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(parentId).ExecuteAsync();
            parentId = translator.Translate(parentId);
            return await _service.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> ReadChildrenAsync(string parentId, int limit = int.MaxValue, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(parentId).ExecuteAsync();
            parentId = translator.Translate(parentId);
            return await _service.ReadChildrenAsync(parentId, limit, token);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(string masterId, string slaveId, TModel item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).Add(item).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            item = translator.Translate(item);
            await _service.UpdateAsync(masterId, slaveId, item, token);
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
            return await _service.UpdateAndReturnAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task DeleteChildrenAsync(string masterId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).ExecuteAsync();
            masterId = translator.Translate(masterId);
            await _service.DeleteChildrenAsync(masterId, token);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(string masterId, string slaveId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).Add(slaveId).ExecuteAsync();
            masterId = translator.Translate(masterId);
            slaveId = translator.Translate(slaveId);
            await _service.DeleteAsync(masterId, slaveId, token);
        }
    }
}