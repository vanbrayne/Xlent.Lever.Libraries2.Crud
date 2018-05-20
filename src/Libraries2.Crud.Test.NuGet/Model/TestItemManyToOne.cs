﻿using Xlent.Lever.Libraries2.Crud.Storage.Model;

namespace Xlent.Lever.Libraries2.Crud.Test.NuGet.Model
{
    /// <summary>
    /// A minimal storable item that implements <see cref="IUniquelyIdentifiable{TId}"/> to be used in testing
    /// </summary>
    public class TestItemManyToOne<TId, TReferenceId> : TestItemManyToOneCreate<TReferenceId>, IUniquelyIdentifiable<TId>
    {
        /// <inheritdoc />
        public TId Id { get; set; }
    }
}
