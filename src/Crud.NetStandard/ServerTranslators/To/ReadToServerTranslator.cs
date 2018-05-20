﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Translation;

namespace Xlent.Lever.Libraries2.Crud.Crud.ServerTranslators.To
{
    /// <summary>
    /// Translate concept values to the server
    /// </summary>
    public class ReadToServerTranslator<TModel> : ServerTranslatorBase, IRead<TModel, string>
    {
        private readonly IRead<TModel, string> _storage;

        /// <inheritdoc />
        public ReadToServerTranslator(IRead<TModel, string> storage, string idConceptName, System.Func<string> getServerNameMethod, ITranslatorService translatorService)
        :base(idConceptName, getServerNameMethod, translatorService)
        {
            _storage = storage;
        }

        /// <inheritdoc />
        public virtual async Task<TModel> ReadAsync(string id, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(id).ExecuteAsync();
            id = translator.Translate(id);
            return await _storage.ReadAsync(id, token);
        }

        /// <inheritdoc />
        public virtual async Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            return await _storage.ReadAllWithPagingAsync(offset, limit, token);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TModel>> ReadAllAsync(int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            return await _storage.ReadAllAsync(limit, token);
        }
    }
}