﻿using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Crud.Crud.Model;
using Xlent.Lever.Libraries2.Crud.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Crud.Interfaces
{
    /// <summary>
    /// Functionality for persisting objects that has no life of their own, but are only relevant with their master.
    /// Examples: A list of rows on an invoice, a list of attributes of an object, the contact details of a person.
    /// </summary>
    public interface ICreateSlaveWithSpecifiedId<TModel, TId> : ICreateSlaveWithSpecifiedId<TModel, TModel, TId>
    {
    }

    /// <summary>
    /// Functionality for persisting objects that has no life of their own, but are only relevant with their master.
    /// Examples: A list of rows on an invoice, a list of attributes of an object, the contact details of a person.
    /// </summary>
    public interface ICreateSlaveWithSpecifiedId<in TModelCreate, TModel, TId>
        where TModel : TModelCreate
    {
        /// <summary>
        /// Same as <see cref="ICreateSlave{TModelCreate,TModel,TId}.CreateAsync"/>, but you can specify the new id.
        /// </summary>
        /// <param name="id">The id to use for the new item.</param>
        /// <param name="item">The item to create in storage.</param>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <returns>The newly created item.</returns>
        Task CreateWithSpecifiedIdAsync(SlaveToMasterId<TId> id, TModelCreate item, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Same as <see cref="ICreateSlave{TModelCreate,TModel,TId}.CreateAndReturnAsync"/>, but you can specify the new id.
        /// </summary>
        /// <param name="id">The id to use for the new item.</param>
        /// <param name="item">The item to store.</param>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <returns>The new item as it was saved, see remarks below.</returns>
        /// <remarks>
        /// If the returned type implements <see cref="IUniquelyIdentifiable{TId}"/>, then the <see cref="IUniquelyIdentifiable{TId}.Id"/> is updated with the new id. 
        /// If it implements <see cref="IOptimisticConcurrencyControlByETag"/>, then the <see cref="IOptimisticConcurrencyControlByETag.Etag"/> is updated..
        /// </remarks>
        /// <seealso cref="IOptimisticConcurrencyControlByETag"/>
        /// <seealso cref="IUniquelyIdentifiable{TId}"/>
        Task<TModel> CreateWithSpecifiedIdAndReturnAsync(SlaveToMasterId<TId> id, TModelCreate item, CancellationToken token = default(CancellationToken));
    }
}
