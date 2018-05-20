﻿using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Translation;

namespace Xlent.Lever.Libraries2.Crud.Crud.ServerTranslators.To
{
    /// <inheritdoc cref="CrdToServerTranslator{TModelCreate, TModel}" />
    public class CrudToServerTranslator<TModel> : CrudToServerTranslator<TModel, TModel>, ICrud<TModel, string>
    {
        /// <inheritdoc />
        public CrudToServerTranslator(ICrud<TModel, string> storage, string idConceptName,
            System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(storage, idConceptName, getServerNameMethod, translatorService)
        {
        }
    }

    /// <inheritdoc cref="CrdToServerTranslator{TModelCreate, TModel}" />
        public class CrudToServerTranslator<TModelCreate, TModel> : CrdToServerTranslator<TModelCreate, TModel>, ICrud<TModelCreate, TModel, string>
            where TModel : TModelCreate
        {
            private readonly ICrud<TModelCreate, TModel, string> _storage;

            /// <inheritdoc />
            public CrudToServerTranslator(ICrud<TModelCreate, TModel, string> storage, string idConceptName, System.Func<string> getServerNameMethod, ITranslatorService translatorService)
                : base(storage, idConceptName, getServerNameMethod, translatorService)
            {
                _storage = storage;
            }

            /// <inheritdoc />
            public async Task UpdateAsync(string id, TModel item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(id).Add(item).ExecuteAsync();
            id = translator.Translate(id);
            item = translator.Translate(item);
            await _storage.UpdateAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(string id, TModel item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            await translator.Add(id).Add(item).ExecuteAsync();
            id = translator.Translate(id);
            item = translator.Translate(item);
            return await _storage.UpdateAndReturnAsync(id, item, token);
        }
    }
}