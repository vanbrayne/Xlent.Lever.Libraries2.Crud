using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xlent.Lever.Libraries2.Crud.Interfaces;
using Xlent.Lever.Libraries2.Crud.MemoryStorage;
using Xlent.Lever.Libraries2.Crud.Test.NuGet.ManyToOne;
using Xlent.Lever.Libraries2.Crud.Test.NuGet.Model;

namespace Xlent.Lever.Libraries2.Crud.NetFramework.Test.Crud.Storage
{
    [TestClass]
    public class MemoryManyToOneTest : TestIManyToOne<Guid, Guid?>
    {
        private ICrud<TestItemId<Guid>, Guid> _oneStorage;
        private ICrudManyToOne<TestItemManyToOneCreate<Guid?>, TestItemManyToOne<Guid, Guid?>, Guid> _crudManyStorage;

        [TestInitialize]
        public void Inititalize()
        {
            _oneStorage = new CrudMemory<TestItemId<Guid>, Guid>();
            _crudManyStorage = new ManyToOneMemory<TestItemManyToOneCreate<Guid?>, TestItemManyToOne<Guid, Guid?>, Guid>(item => item.ParentId);
        }

        /// <inheritdoc />
        protected override ICrudManyToOne<TestItemManyToOneCreate<Guid?>, TestItemManyToOne<Guid, Guid?>, Guid>
            CrudManyStorageRecursive => null;

        /// <inheritdoc />
        protected override ICrudManyToOne<TestItemManyToOneCreate<Guid?>, TestItemManyToOne<Guid, Guid?>, Guid>
            CrudManyStorageNonRecursive => _crudManyStorage;

        /// <inheritdoc />
        protected override ICrud<TestItemId<Guid>, Guid> OneStorage => _oneStorage;
    }
}
