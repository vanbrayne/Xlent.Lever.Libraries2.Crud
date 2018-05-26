using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Translation;
using Xlent.Lever.Libraries2.Crud.Model;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.ClientTranslators
{
    /// <inheritdoc cref="CrudClientTranslator{TModelCreate, TModel}" />
    public class CrudClientTranslator<TModel> : 
        CrudClientTranslator<TModel, TModel>, 
        ICrud<TModel, string>
    {
        /// <inheritdoc />
        public CrudClientTranslator(ICrudable service, string idConceptName,
            System.Func<string> getClientNameMethod, ITranslatorService translatorService)
            : base(service, idConceptName, getClientNameMethod, translatorService)
        {
        }
    }

    /// <inheritdoc cref="ClientTranslatorBase" />
    public class CrudClientTranslator<TModelCreate, TModel> : 
        ClientTranslatorBase, 
        ICrud<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private ICrud<TModelCreate, TModel, string> _service;

        /// <inheritdoc />
        public CrudClientTranslator(ICrudable service, string idConceptName, System.Func<string> getClientNameMethod, ITranslatorService translatorService)
            : base(idConceptName, getClientNameMethod, translatorService)
        {
            _service = new CrudPassThrough<TModelCreate, TModel, string>(service);
        }

        /// <inheritdoc />
        public async Task<string> CreateAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            item = translator.DecorateItem(item);
            var decoratedId = await _service.CreateAsync(item, token);
            // This is a new id, so there is no purpose in translating it.
            return decoratedId;
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            item = translator.DecorateItem(item);
            var decoratedResult = await _service.CreateAndReturnAsync(item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(string id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            item = translator.DecorateItem(item);
            await _service.CreateWithSpecifiedIdAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string id, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            item = translator.DecorateItem(item);
            var decoratedResult = await _service.CreateWithSpecifiedIdAndReturnAsync(id, item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public virtual async Task<TModel> ReadAsync(string id, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            var decoratedResult = await _service.ReadAsync(id, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public virtual async Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            var decoratedResult = await _service.ReadAllWithPagingAsync(offset, limit, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TModel>> ReadAllAsync(int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            var decoratedResult = await _service.ReadAllAsync(limit, token);
            var decoratedArray = decoratedResult as TModel[] ?? decoratedResult.ToArray();
            await translator.Add(decoratedArray).ExecuteAsync();
            return translator.Translate(decoratedArray);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(string id, TModel item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            item = translator.DecorateItem(item);
            await _service.UpdateAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(string id, TModel item, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            item = translator.DecorateItem(item);
            var decoratedResult = await _service.UpdateAndReturnAsync(id, item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public Task DeleteAsync(string id, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            return _service.DeleteAsync(id, token);
        }

        /// <inheritdoc />
        public Task DeleteAllAsync(CancellationToken token = new CancellationToken())
        {
            return _service.DeleteAllAsync(token);
        }

        /// <inheritdoc />
        public Task<Lock> ClaimLockAsync(string id, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            id = translator.Decorate(IdConceptName, id);
            return _service.ClaimLockAsync(id, token);
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(Lock @lock, CancellationToken token = default(CancellationToken))
        {
            return _service.ReleaseLockAsync(@lock, token);
        }
    }
}