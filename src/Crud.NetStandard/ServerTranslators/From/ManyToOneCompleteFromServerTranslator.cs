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
    public class ManyToOneCrudFromServerTranslator<TModel> : ManyToOneCrudFromServerTranslator<TModel, TModel>,
        IManyToOneCrud<TModel, string>
    {
        /// <inheritdoc />
        public ManyToOneCrudFromServerTranslator(IManyToOneCrud<TModel, string> storage, string idConceptName,
            System.Func<string> getServerNameMethod)
            : base(storage, idConceptName, getServerNameMethod)
        {
        }
    }

    /// <summary>
    /// Decorate values from the server into concept values.
    /// </summary>
    public class ManyToOneCrudFromServerTranslator<TModelCreate, TModel> : CrudFromServerTranslator<TModelCreate, TModel>, IManyToOneCrud<TModelCreate, TModel, string> 
        where TModel : TModelCreate
    {
        private readonly IManyToOneCrud<TModelCreate, TModel, string> _storage;

        /// <inheritdoc />
        public ManyToOneCrudFromServerTranslator(IManyToOneCrud<TModelCreate, TModel, string> storage, string idConceptName, System.Func<string> getServerNameMethod)
            : base(storage, idConceptName, getServerNameMethod)
        {
            _storage = storage;
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
        public async Task DeleteChildrenAsync(string parentId, CancellationToken token = default(CancellationToken))
        {
            await _storage.DeleteChildrenAsync(parentId, token);
        }
    }
}