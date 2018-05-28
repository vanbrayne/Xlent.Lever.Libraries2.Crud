using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Model;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.ServerTranslators.From
{
    /// <inheritdoc cref="CrudFromServerTranslator{TModelCreate, TModel}" />
    public class CrudFromServerTranslator<TModel> : CrudFromServerTranslator<TModel, TModel>, ICrud<TModel, string>
    {
        /// <inheritdoc />
        public CrudFromServerTranslator(ICrudable service, string idConceptName,
            System.Func<string> getServerNameMethod)
            : base(service, idConceptName, getServerNameMethod)
        {
        }
    }

    /// <inheritdoc cref="ServerTranslatorBase" />
    public class CrudFromServerTranslator<TModelCreate, TModel> : ServerTranslatorBase, ICrud<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private readonly ICrud<TModelCreate, TModel, string> _service;

        /// <inheritdoc />
        public CrudFromServerTranslator(ICrudable service, string idConceptName, System.Func<string> getServerNameMethod)
            : base(idConceptName, getServerNameMethod)
        {
            _service = new CrudPassThrough<TModelCreate, TModel, string>(service);
        }

        /// <inheritdoc />
        public async Task<string> CreateAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var id = await _service.CreateAsync(item, token);
            var translator = CreateTranslator();
            return translator.Decorate(IdConceptName, id);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(TModelCreate item, CancellationToken token = new CancellationToken())
        {
            var decoratedResult = await _service.CreateAndReturnAsync(item, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(decoratedResult);
        }

        /// <inheritdoc />
        public Task CreateWithSpecifiedIdAsync(string id, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            return _service.CreateWithSpecifiedIdAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string id, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var decoratedResult = await _service.CreateWithSpecifiedIdAndReturnAsync(id, item, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(decoratedResult);
        }

        /// <inheritdoc />
        public virtual async Task<TModel> ReadAsync(string id, CancellationToken token = default(CancellationToken))
        {
            var result = await _service.ReadAsync(id, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(result);
        }

        /// <inheritdoc />
        public virtual async Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = default(CancellationToken))
        {
            var result = await _service.ReadAllWithPagingAsync(offset, limit, token);
            var translator = CreateTranslator();
            return translator.DecoratePage(result);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TModel>> ReadAllAsync(int limit = int.MaxValue, CancellationToken token = default(CancellationToken))
        {
            var result = await _service.ReadAllAsync(limit, token);
            var translator = CreateTranslator();
            return translator.DecorateItems(result);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(string id, TModel item, CancellationToken token = new CancellationToken())
        {
            await _service.UpdateAsync(id, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(string id, TModel item, CancellationToken token = new CancellationToken())
        {
            var result = await _service.UpdateAndReturnAsync(id, item, token);
            var translator = CreateTranslator();
            return translator.DecorateItem(result);
        }

        /// <inheritdoc />
        public Task DeleteAsync(string id, CancellationToken token = new CancellationToken())
        {
            return _service.DeleteAsync(id, token);
        }

        /// <inheritdoc />
        public Task DeleteAllAsync(CancellationToken token = new CancellationToken())
        {
            return _service.DeleteAllAsync(token);
        }

        /// <inheritdoc />
        public Task<Lock<string>> ClaimLockAsync(string id, CancellationToken token = default(CancellationToken))
        {
            return _service.ClaimLockAsync(id, token);
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(string id, string lockId, CancellationToken token = default(CancellationToken))
        {
            return _service.ReleaseLockAsync(id, lockId, token);
        }
    }
}