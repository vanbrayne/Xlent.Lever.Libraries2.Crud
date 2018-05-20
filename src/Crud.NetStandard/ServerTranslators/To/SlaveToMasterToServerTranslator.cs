﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Translation;

namespace Xlent.Lever.Libraries2.Crud.Crud.ServerTranslators.To
{
    /// <summary>
    /// Translate concept values to the server
    /// </summary>
    public class SlaveToMasterToServerTranslator<TModel> :
        SlaveToMasterToServerTranslator<TModel, TModel>,
        ISlaveToMasterCrud<TModel, string>
    {
        /// <inheritdoc />
        public SlaveToMasterToServerTranslator(ISlaveToMasterCrud<TModel, string> storage,
            System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(null, getServerNameMethod, translatorService)
        {
        }
    }

    /// <summary>
    /// Translate concept values to the server
    /// </summary>
    public class SlaveToMasterToServerTranslator<TModelCreate, TModel> : 
        ServerTranslatorBase, 
        ISlaveToMasterCrud<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ISlaveToMasterCrud<TModelCreate, TModel, string> _storage;

        /// <inheritdoc />
        public SlaveToMasterToServerTranslator(ISlaveToMasterCrud<TModelCreate, TModel, string> storage, System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(null, getServerNameMethod, translatorService)
        {
            _storage = storage;
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
        public async Task DeleteChildrenAsync(string masterId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(masterId).ExecuteAsync();
            masterId = translator.Translate(masterId);
            await _storage.DeleteChildrenAsync(masterId, token);
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
        public async Task CreateWithSpecifiedIdAsync(SlaveToMasterId<string> id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(id).Add(item).ExecuteAsync();
            id = translator.Translate(id);
            item = translator.Translate(item);
            await _storage.CreateWithSpecifiedIdAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(SlaveToMasterId<string> id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(id).Add(item).ExecuteAsync();
            id = translator.Translate(id);
            item = translator.Translate(item);
            return await _storage.CreateWithSpecifiedIdAndReturnAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(SlaveToMasterId<string> id, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(id).ExecuteAsync();
            id = translator.Translate(id);
            await _storage.DeleteAsync(id, token);
        }

        /// <inheritdoc />
        public Task DeleteAllAsync(CancellationToken token = default(CancellationToken))
        {
            return _storage.DeleteAllAsync(token);
        }
    }
}