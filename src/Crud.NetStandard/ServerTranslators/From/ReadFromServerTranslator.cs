﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Crud.ServerTranslators.From
{
    /// <summary>
    /// Decorate values from the server into concept values.
    /// </summary>
    public class ReadFromServerTranslator<TModel> : ServerTranslatorBase, IRead<TModel, string>
    {
        private readonly IRead<TModel, string> _storage;

        /// <inheritdoc />
        public ReadFromServerTranslator(IRead<TModel, string> storage, string idConceptName, System.Func<string> getServerNameMethod)
        :base(idConceptName, getServerNameMethod)
        {
            _storage = storage;
        }

        /// <inheritdoc />
        public virtual async Task<TModel> ReadAsync(string id, CancellationToken token = default(CancellationToken))
        {
            var result = await _storage.ReadAsync(id, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(result);   
        }

        /// <inheritdoc />
        public virtual async Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            var result =  await _storage.ReadAllWithPagingAsync(offset, limit, token);
            var translator = CreateTranslator();
            return translator.DecoratePage(result);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TModel>> ReadAllAsync(int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            var result = await _storage.ReadAllAsync(limit, token);
            var translator = CreateTranslator();
            return translator.DecorateItems(result);
        }
    }
}