﻿using System.Threading;
using System.Threading.Tasks;
using Xlent.Lever.Libraries2.Core.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <summary>
    /// Can create items."/>.
    /// </summary>
    /// <typeparam name="TModel">The type for creating objects in persistant storage.</typeparam>
    /// <typeparam name="TId">The type for the id of the stored objects.</typeparam>
    public interface ICreateAndReturn<TModel, TId> : ICreateAndReturn<TModel, TModel, TId>
    {
    }

    /// <summary>
    /// Can create items."/>.
    /// </summary>
    /// <typeparam name="TModelCreate">The type for creating objects in persistant storage.</typeparam>
    /// <typeparam name="TModel">The type of objects that are returned from persistant storage.</typeparam>
    /// <typeparam name="TId">The type for the <see cref="IUniquelyIdentifiable{TId}.Id"/> property.</typeparam>
    public interface ICreateAndReturn<in TModelCreate, TModel, TId> : ICrudable<TModel, TId>
    where TModel : TModelCreate
    {
        /// <summary>
        /// Creates a new item in storage and returns the final result.
        /// </summary>
        /// <param name="item">The item to store.</param>
        /// <param name="token">Propagates notification that operations should be canceled</param>
        /// <returns>The new item as it was saved, see remarks below.</returns>
        /// <remarks>
        /// If the returned type implements <see cref="IUniquelyIdentifiable{TId}"/>, then the <see cref="IUniquelyIdentifiable{TId}.Id"/> is updated with the new id. 
        /// If it implements <see cref="IOptimisticConcurrencyControlByETag"/>, then the <see cref="IOptimisticConcurrencyControlByETag.Etag"/> is updated..
        /// </remarks>
        /// <seealso cref="IOptimisticConcurrencyControlByETag"/>
        /// <seealso cref="IUniquelyIdentifiable{TId}"/>
        Task<TModel> CreateAndReturnAsync(TModelCreate item, CancellationToken token = default(CancellationToken));
    }
}
