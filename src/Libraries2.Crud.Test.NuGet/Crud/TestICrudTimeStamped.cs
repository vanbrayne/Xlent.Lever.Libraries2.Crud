﻿using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.Test.NuGet.Crd;
using Xlent.Lever.Libraries2.Crud.Test.NuGet.Model;

namespace Xlent.Lever.Libraries2.Crud.Test.NuGet.Crud
{
    /// <summary>
    /// Tests for testing any storage that implements <see cref="ICrud{TModelCreate,TModel,TId}"/>"/>
    /// </summary>
    [TestClass]
    public abstract class TestICrudTimeStamped<TId> : TestICrdTimeStamped<TId>
    {
        protected override ICrud<TestItemBare, TestItemTimestamped<TId>, TId> CrdStorage => CrudStorage;

        /// <summary>
        /// Create an item with an id.
        /// </summary>
        [TestMethod]
        public async Task Update_Read_Async()
        {
            var id = await CreateItemAsync(TypeOfTestDataEnum.Variant1);
            var updateItem = await UpdateItemAsync(id, TypeOfTestDataEnum.Variant2);
            var readItem = await ReadItemAsync(id);
            if (!updateItem.Equals(readItem))
            {
                updateItem.RecordUpdatedAt = readItem.RecordUpdatedAt;
            }
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(updateItem, readItem);
        }
    }
}

