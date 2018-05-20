﻿using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Translation;

namespace Xlent.Lever.Libraries2.Crud.Crud.ClientTranslators
{
    /// <inheritdoc cref="CrdClientTranslator{TModel}" />
    public class CrdClientTranslator<TModel> : CrdClientTranslator<TModel, TModel>, ICrd<TModel, string>
    {
        /// <inheritdoc />
        public CrdClientTranslator(ICrd<TModel, string> storage, string idConceptName,
            System.Func<string> getClientNameMethod, ITranslatorService translatorService)
            : base(storage, idConceptName, getClientNameMethod, translatorService)
        {
        }
    }

    /// <summary>
    /// Decorate from client and translate to client.
    /// </summary>
    public class CrdClientTranslator<TModelCreate, TModel> : ReadClientTranslator<TModel>, ICrd<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrd<TModelCreate, TModel, string> _storage;

        /// <inheritdoc />
        public CrdClientTranslator(ICrd<TModelCreate, TModel, string> storage, string idConceptName, System.Func<string> getClientNameMethod, ITranslatorService translatorService)
            : base(storage, idConceptName, getClientNameMethod, translatorService)
        {
            _storage = storage;
        }

        /// <inheritdoc />
        public async Task<string> CreateAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            item = translator.DecorateItem(item);
            var decoratedId = await _storage.CreateAsync(item, token);
            // This is a new id, so there is no purpose in translating it.
            return decoratedId;
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            item = translator.DecorateItem(item);
            var decoratedResult = await _storage.CreateAndReturnAsync(item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(string id, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            await _storage.DeleteAsync(id, token);
        }

        /// <inheritdoc />
        public async Task DeleteAllAsync(CancellationToken token = new CancellationToken())
        {
            await _storage.DeleteAllAsync(token);
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(string id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            item = translator.DecorateItem(item);
            await _storage.CreateWithSpecifiedIdAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string id, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            item = translator.DecorateItem(item);
            var decoratedResult = await _storage.CreateWithSpecifiedIdAndReturnAsync(id, item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public async Task<Lock> ClaimLockAsync(string id, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            return await _storage.ClaimLockAsync(id, token);
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(Lock @lock, CancellationToken token = default(CancellationToken))
        {
            return _storage.ReleaseLockAsync(@lock, token);
        }
    }
}