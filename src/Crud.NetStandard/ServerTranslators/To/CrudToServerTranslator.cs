﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Translation;
using Xlent.Lever.Libraries2.Crud.Model;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.ServerTranslators.To
{
    /// <inheritdoc cref="CrudToServerTranslator{TModelCreate, TModel}" />
    public class CrudToServerTranslator<TModel> : CrudToServerTranslator<TModel, TModel>, ICrud<TModel, string>
    {
        /// <inheritdoc />
        public CrudToServerTranslator(ICrudable service, string idConceptName,
            System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(service, idConceptName, getServerNameMethod, translatorService)
        {
        }
    }

    /// <inheritdoc cref="ServerTranslatorBase" />
    public class CrudToServerTranslator<TModelCreate, TModel> : ServerTranslatorBase, ICrud<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrud<TModelCreate, TModel, string> _service;

        /// <inheritdoc />
        public CrudToServerTranslator(ICrudable service, string idConceptName, System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(idConceptName, getServerNameMethod, translatorService)
        {
            _service = new CrudPassThrough<TModelCreate, TModel, string>(service);
        }

        /// <inheritdoc />
        public async Task<string> CreateAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(item).ExecuteAsync();
            item = translator.Translate(item);
            return await _service.CreateAsync(item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(item).ExecuteAsync();
            item = translator.Translate(item);
            return await _service.CreateAndReturnAsync(item, token);
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(string id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(id).Add(item).ExecuteAsync();
            id = translator.Translate(id);
            item = translator.Translate(item);
            await _service.CreateWithSpecifiedIdAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string id, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(id).Add(item).ExecuteAsync();
            id = translator.Translate(id);
            item = translator.Translate(item);
            return await _service.CreateWithSpecifiedIdAndReturnAsync(id, item, token);
        }

        /// <inheritdoc />
        public virtual async Task<TModel> ReadAsync(string id, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(id).ExecuteAsync();
            id = translator.Translate(id);
            return await _service.ReadAsync(id, token);
        }

        /// <inheritdoc />
        public virtual async Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            return await _service.ReadAllWithPagingAsync(offset, limit, token);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TModel>> ReadAllAsync(int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            return await _service.ReadAllAsync(limit, token);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(string id, TModel item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(id).Add(item).ExecuteAsync();
            id = translator.Translate(id);
            item = translator.Translate(item);
            await _service.UpdateAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(string id, TModel item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(id).Add(item).ExecuteAsync();
            id = translator.Translate(id);
            item = translator.Translate(item);
            return await _service.UpdateAndReturnAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(string id, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(id).ExecuteAsync();
            id = translator.Translate(id);
            await _service.DeleteAsync(id, token);
        }

        /// <inheritdoc />
        public Task DeleteAllAsync(CancellationToken token = new CancellationToken())
        {
            return _service.DeleteAllAsync(token);
        }

        /// <inheritdoc />
        public async Task<Lock<string>> ClaimLockAsync(string id, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(id).ExecuteAsync();
            id = translator.Translate(id);
            return await _service.ClaimLockAsync(id, token);
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(string id, string lockId, CancellationToken token = default(CancellationToken))
        {
            return _service.ReleaseLockAsync(id, lockId, token);
        }
    }
}