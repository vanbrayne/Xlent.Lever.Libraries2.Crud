using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Storage.Logic;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Model;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.Encrypt
{
    /// <inheritdoc cref="EncryptBase{TModel,TId}" />
    public class CrudEncrypt <TModel, TId>: 
        EncryptBase<TModel, TId>, 
        ICrud<TModel, TId>
    {
        private readonly ICrud<Core.Storage.Logic.StorableAsByteArray<TModel, TId>, TId> _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="symmetricEncryptionKey"></param>
        public CrudEncrypt(ICrudable service, byte[] symmetricEncryptionKey)
        :base(symmetricEncryptionKey)
        {
            _service = new CrudPassThrough<StorableAsByteArray<TModel, TId>, TId>(service);
        }

        /// <inheritdoc />
        public async Task<TId> CreateAsync(TModel item, CancellationToken token = new CancellationToken())
        {
            var storedItem = Encrypt(item);
            return await _service.CreateAsync(storedItem, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(TModel item, CancellationToken token = new CancellationToken())
        {
            var storedItem = Encrypt(item);
            storedItem = await _service.CreateAndReturnAsync(storedItem, token);
            return Decrypt(storedItem);
        }

        /// <inheritdoc />
        public async Task CreateWithSpecifiedIdAsync(TId id, TModel item, CancellationToken token = default(CancellationToken))
        {
            var storedItem = Encrypt(item);
            await _service.CreateWithSpecifiedIdAsync(id, storedItem, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(TId id, TModel item, CancellationToken token = default(CancellationToken))
        {
            var storedItem = Encrypt(item);
            storedItem = await _service.CreateWithSpecifiedIdAndReturnAsync(id, storedItem, token);
            return Decrypt(storedItem);
        }

        /// <inheritdoc />
        public async Task<TModel> ReadAsync(TId id, CancellationToken token = new CancellationToken())
        {
            var storedItem = await _service.ReadAsync(id, token);
            return Decrypt(storedItem);
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = new CancellationToken())
        {
            var page = await _service.ReadAllWithPagingAsync(offset, limit, token);
            return new PageEnvelope<TModel>(page.PageInfo, page.Data.Select(Decrypt));

        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> ReadAllAsync(int limit = int.MaxValue, CancellationToken token = new CancellationToken())
        {
            var storedItems = await _service.ReadAllAsync(limit, token);
            return storedItems.Select(Decrypt);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(TId id, TModel item, CancellationToken token = new CancellationToken())
        {
            var storedItem = Encrypt(item);
            await _service.UpdateAsync(id, storedItem, token);
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(TId id, TModel item, CancellationToken token = new CancellationToken())
        {
            var storedItem = Encrypt(item);
            storedItem = await _service.UpdateAndReturnAsync(id, storedItem, token);
            return Decrypt(storedItem);
        }

        /// <inheritdoc />
        public Task DeleteAsync(TId id, CancellationToken token = new CancellationToken())
        {
            return _service.DeleteAsync(id, token);
        }

        /// <inheritdoc />
        public Task DeleteAllAsync(CancellationToken token = new CancellationToken())
        {
            return _service.DeleteAllAsync(token);
        }

        /// <inheritdoc />
        public Task<Lock<TId>> ClaimLockAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            return _service.ClaimLockAsync(id, token);
        }

        /// <inheritdoc />
        public Task ReleaseLockAsync(TId id, TId lockId, CancellationToken token = default(CancellationToken))
        {
            return _service.ReleaseLockAsync(id, lockId, token);
        }
    }
}