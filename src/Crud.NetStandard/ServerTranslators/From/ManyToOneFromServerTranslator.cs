using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.ServerTranslators.From
{
    /// <inheritdoc cref="ManyToOneFromServerTranslator{TModelCreate, TModel}" />
    public class ManyToOneFromServerTranslator<TModel> :
        ManyToOneFromServerTranslator<TModel, TModel>,
        ICrudManyToOne<TModel, string>
    {

        /// <inheritdoc />
        public ManyToOneFromServerTranslator(ICrudable<TModel, string> service, string idConceptName,
            System.Func<string> getServerNameMethod)
            : base(service, idConceptName, getServerNameMethod)
        {
        }
    }

    /// <inheritdoc cref="CrudFromServerTranslator{TModelCreate, TModel}" />
    public class ManyToOneFromServerTranslator<TModelCreate, TModel> :
        CrudFromServerTranslator<TModelCreate, TModel>,
        ICrudManyToOne<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrudManyToOne<TModelCreate, TModel, string> _service;

        /// <inheritdoc />
        public ManyToOneFromServerTranslator(ICrudable<TModel, string> service, string idConceptName, System.Func<string> getServerNameMethod)
            : base(service, idConceptName, getServerNameMethod)
        {
            _service = new ManyToOnePassThrough<TModelCreate, TModel, string>(service);
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(string parentId, int offset, int? limit = null,
        CancellationToken token = new CancellationToken())
        {
            var result = await _service.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(result);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> ReadChildrenAsync(string parentId, int limit = int.MaxValue, CancellationToken token = new CancellationToken())
        {
            var result = await _service.ReadChildrenAsync(parentId, limit, token);
            var translator = CreateTranslator();
            return translator.DecorateItems(result);
        }

        /// <inheritdoc />
        public Task DeleteChildrenAsync(string parentId, CancellationToken token = default(CancellationToken))
        {
            return _service.DeleteChildrenAsync(parentId, token);
        }
    }
}