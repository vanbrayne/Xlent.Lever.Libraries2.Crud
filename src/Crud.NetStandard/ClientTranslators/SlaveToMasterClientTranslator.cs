﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Core.Storage.Model;
using Xlent.Lever.Libraries2.Core.Translation;
using Xlent.Lever.Libraries2.Crud.PassThrough;

namespace Xlent.Lever.Libraries2.Crud.ClientTranslators
{
    /// <inheritdoc cref="SlaveToMasterClientTranslator{TModelCreate, TModel}" />
    public class SlaveToMasterClientTranslator<TModel> : 
        SlaveToMasterClientTranslator<TModel, TModel>,
        ICrudSlaveToMaster<TModel, string>
    {
        /// <inheritdoc />
        public SlaveToMasterClientTranslator(ICrudable service,
            string masterIdConceptName, string slaveIdConceptName, System.Func<string> getClientNameMethod,
            ITranslatorService translatorService)
            : base(service, masterIdConceptName, slaveIdConceptName, getClientNameMethod, translatorService)
        {
        }
    }

    /// <inheritdoc cref="ClientTranslatorBase" />
    public class SlaveToMasterClientTranslator<TModelCreate, TModel> :
        ClientTranslatorBase,
        ICrudSlaveToMaster<TModelCreate, TModel, string>
        where TModel : TModelCreate
    {
        private ICrudSlaveToMaster<TModelCreate, TModel, string> _service { get; }
        private readonly string _masterIdConceptName;

        /// <inheritdoc />
        public SlaveToMasterClientTranslator(ICrudable service, string masterIdConceptName, string slaveIdConceptName, System.Func<string> getClientNameMethod, ITranslatorService translatorService)
            : base(slaveIdConceptName, getClientNameMethod, translatorService)
        {
            _masterIdConceptName = masterIdConceptName;
            _service = new SlaveToMasterPassThrough<TModelCreate, TModel, string>(service);
        }

        /// <inheritdoc />
        public async Task<SlaveToMasterId<string>> CreateAsync(string masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            item = translator.DecorateItem(item);
            var decoratedResult = await _service.CreateAsync(masterId, item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateAndReturnAsync(string masterId, TModelCreate item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            item = translator.DecorateItem(item);
            var decoratedResult = await _service.CreateAndReturnAsync(masterId, item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public Task CreateWithSpecifiedIdAsync(string masterId, string slaveId, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            slaveId = translator.Decorate(IdConceptName, slaveId);
            item = translator.DecorateItem(item);
            return _service.CreateWithSpecifiedIdAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> CreateWithSpecifiedIdAndReturnAsync(string masterId, string slaveId, TModelCreate item,
            CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            slaveId = translator.Decorate(IdConceptName, slaveId);
            item = translator.DecorateItem(item);
            var decoratedResult = await _service.CreateWithSpecifiedIdAndReturnAsync(masterId, slaveId, item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public async Task<TModel> ReadAsync(string masterId, string slaveId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            slaveId = translator.Decorate(IdConceptName, slaveId);
            var decoratedResult = await _service.ReadAsync(masterId, slaveId, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public async Task<PageEnvelope<TModel>> ReadChildrenWithPagingAsync(string parentId, int offset, int? limit = null,
        CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            parentId = translator.Decorate(_masterIdConceptName, parentId);
            var decoratedResult = await _service.ReadChildrenWithPagingAsync(parentId, offset, limit, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> ReadChildrenAsync(string parentId, int limit = int.MaxValue, CancellationToken token = new CancellationToken())
        {
            var translator = CreateTranslator();
            parentId = translator.Decorate(_masterIdConceptName, parentId);
            var decoratedResult = await _service.ReadChildrenAsync(parentId, limit, token);
            var array = decoratedResult as TModel[] ?? decoratedResult.ToArray();
            await translator.Add(array).ExecuteAsync();
            return translator.Translate(array);
        }

        /// <inheritdoc />
        public Task UpdateAsync(string masterId, string slaveId, TModel item, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            slaveId = translator.Decorate(IdConceptName, slaveId);
            item = translator.DecorateItem(item);
            return _service.UpdateAsync(masterId, slaveId, item, token);
        }

        /// <inheritdoc />
        public async Task<TModel> UpdateAndReturnAsync(string masterId, string slaveId, TModel item,
            CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            slaveId = translator.Decorate(IdConceptName, slaveId);
            item = translator.DecorateItem(item);
            var decoratedResult = await _service.UpdateAndReturnAsync(masterId, slaveId, item, token);
            await translator.Add(decoratedResult).ExecuteAsync();
            return translator.Translate(decoratedResult);
        }

        /// <inheritdoc />
        public Task DeleteAsync(string masterId, string slaveId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            slaveId = translator.Decorate(IdConceptName, slaveId);
            return _service.DeleteAsync(masterId, slaveId, token);
        }

        /// <inheritdoc />
        public Task DeleteChildrenAsync(string masterId, CancellationToken token = default(CancellationToken))
        {
            var translator = CreateTranslator();
            masterId = translator.Decorate(_masterIdConceptName, masterId);
            return _service.DeleteChildrenAsync(masterId, token);
        }
    }
}