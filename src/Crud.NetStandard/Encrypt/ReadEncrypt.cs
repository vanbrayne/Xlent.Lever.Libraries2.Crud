﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Security;
using Xlent.Lever.Libraries2.Crud.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Crud.Encrypt
{
    /// <inheritdoc />
    public class ReadEncrypt <TModel, TId>: IRead<TModel, TId>
    {
        private readonly SymmetricCrypto _symmetricCrypto;
        private readonly IRead<Storage.Logic.StorableAsByteArray<TModel, TId>, TId> _storage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="symmetricEncryptionKey"></param>
        public ReadEncrypt(IRead<Storage.Logic.StorableAsByteArray<TModel, TId>, TId> storage, byte[] symmetricEncryptionKey)
        {
            _storage = storage;
            _symmetricCrypto = new SymmetricCrypto(symmetricEncryptionKey);
        }

        /// <inheritdoc />
        public async Task<TModel> ReadAsync(TId id, CancellationToken token = new CancellationToken())
        {
            var storedItem = await _storage.ReadAsync(id, token);
            return Decrypt(storedItem);
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TModel>> ReadAllWithPagingAsync(int offset, int? limit = null, CancellationToken token = new CancellationToken())
        {
            var page = await _storage.ReadAllWithPagingAsync(offset, limit, token);
            return new PageEnvelope<TModel>(page.PageInfo, page.Data.Select(Decrypt));
            
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> ReadAllAsync(int limit = int.MaxValue, CancellationToken token = new CancellationToken())
        {
            var storedItems = await _storage.ReadAllAsync(limit, token);
            return storedItems.Select(Decrypt);
        }

        #region Protected
        /// <summary>
        /// Encrypt an <paramref name="item"/> into a StorableAsByteArray.
        /// </summary>
        protected Storage.Logic.StorableAsByteArray<TModel, TId> Encrypt(TModel item)
        {
            var storedItem = new Storage.Logic.StorableAsByteArray<TModel, TId>
            {
                Data = item
            };
            storedItem.ByteArray = _symmetricCrypto.Encrypt(storedItem.ByteArray);
            return storedItem;
        }

        /// <summary>
        /// Returns a <paramref name="storedItem"/> decryped into an item.
        /// </summary>
        protected TModel Decrypt(IStorableAsByteArray<TModel, TId> storedItem)
        {
            storedItem.ByteArray = _symmetricCrypto.Decrypt(storedItem.ByteArray);
            return storedItem.Data;
        }
        #endregion
    }
}