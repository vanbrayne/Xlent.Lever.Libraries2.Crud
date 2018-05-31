using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Core.Translation;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.ServerTranslators.To
{
    /// <inheritdoc cref="ManyToOneToServerTranslator{TModelCreate, TModel}" />
    public class ManyToOneToServerTranslator<TModel> : 
        ManyToOneToServerTranslator<TModel, TModel>, 
        ICrudManyToOne<TModel, string>
    {

        /// <inheritdoc />
        public ManyToOneToServerTranslator(ICrudable<TModel, string> service, string idConceptName,
            System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(service, idConceptName, getServerNameMethod, translatorService)
        {
        }
    }

    /// <inheritdoc cref="CrudToServerTranslator{TModelCreate, TModel}" />
    public class ManyToOneToServerTranslator<TModelCreate, TModel> :
        CrudToServerTranslator<TModelCreate, TModel>,
        ICrudManyToOne<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrudManyToOne<TModelCreate, TModel, string> _service;

        /// <inheritdoc />
        public ManyToOneToServerTranslator(ICrudable<TModel, string> service, string idConceptName, System.Func<string> getServerNameMethod, ITranslatorService translatorService)
            : base(service, idConceptName, getServerNameMethod, translatorService)
        {
            _service = new ManyToOnePassThrough<TModelCreate, TModel, string>(service);
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
        public async Task DeleteChildrenAsync(string parentId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            await translator.Add(parentId).ExecuteAsync();
            parentId = translator.Translate(parentId);
            await _service.DeleteChildrenAsync(parentId, token);
        }
    }
}