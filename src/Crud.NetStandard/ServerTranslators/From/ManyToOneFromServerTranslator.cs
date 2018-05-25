using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;

namespace Xlent.Lever.Libraries2.Crud.ServerTranslators.From
{
    /// <summary>
    /// Decorate values from the server into concept values.
    /// </summary>
    public class ManyToOneFromServerTranslator<TModel> : ServerTranslatorBase, IManyToOne<TModel, string>
    {
        private readonly IManyToOne<TModel, string> _storage;

        /// <inheritdoc />
        public ManyToOneFromServerTranslator(IManyToOne<TModel, string> storage, string idConceptName, System.Func<string> getServerNameMethod)
        :base(idConceptName, getServerNameMethod)
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
    }
}