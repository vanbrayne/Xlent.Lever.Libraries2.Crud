﻿using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;

namespace Xlent.Lever.Libraries2.Crud.Crud.ServerTranslators.From
{
    /// <inheritdoc cref="ReadFromServerTranslator{TModel}" />
    public class CrdFromServerTranslator<TModel> : CrdFromServerTranslator<TModel, TModel>, ICrd<TModel, string>
    {
        /// <inheritdoc />
        public CrdFromServerTranslator(ICrd<TModel, string> storage, string idConceptName,
            System.Func<string> getServerNameMethod)
            : base(storage, idConceptName, getServerNameMethod)
        {
        }
    }

    /// <inheritdoc cref="ReadFromServerTranslator{TModel}" />
    public class CrdFromServerTranslator<TModelCreate, TModel> : ReadFromServerTranslator<TModel>, ICrd<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrd<TModelCreate, TModel, string> _storage;

        /// <inheritdoc />
        public CrdFromServerTranslator(ICrd<TModelCreate, TModel, string> storage, string idConceptName, System.Func<string> getServerNameMethod)
            : base(storage, idConceptName, getServerNameMethod)
        {
            _storage = storage;
        }

        /// <inheritdoc />
        public async Task<string> CreateAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var id = await _storage.CreateAsync(item, token);
            var translator = CreateTranslator();
            return translator.Decorate(IdConceptName, id);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var decoratedResult = await _storage.CreateAndReturnAsync(item, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(decoratedResult);
        }

        /// <inheritdoc />
        public Task DeleteAsync(string id, CancellationToken token = new CancellationToken())
        {
            return _storage.DeleteAsync(id, token);
        }

        /// <inheritdoc />
        public Task DeleteAllAsync(CancellationToken token = new CancellationToken())
        {
            return _storage.DeleteAllAsync(token);
        }

        /// <inheritdoc />
        public Task CreateWithSpecifiedIdAsync(string id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            return _storage.CreateWithSpecifiedIdAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string id, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var decoratedResult = await _storage.CreateWithSpecifiedIdAndReturnAsync(id, item, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(decoratedResult);
        }

        /// <inheritdoc />
        public Task<Lock> ClaimLockAsync(string id, CancellationToken token = default(CancellationToken))
        {
            return _storage.ClaimLockAsync(id, token);
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(Lock @lock, CancellationToken token = default(CancellationToken))
        {
            return _storage.ReleaseLockAsync(@lock, token);
        }
    }
}