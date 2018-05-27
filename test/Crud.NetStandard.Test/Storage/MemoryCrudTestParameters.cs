using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xlent.Lever.Libraries2.Core.Application;
using Xlent.Lever.Libraries2.Crud.MemoryStorage;
using Xlent.Lever.Libraries2.Crud.Test.NuGet.Crd;
using Xlent.Lever.Libraries2.Crud.Test.NuGet.Model;

namespace Xlent.Lever.Libraries2.Crud.NetFramework.Test.Crud.Storage
{
    [TestClass]
    public class MemoryCrudTestParameters : TestParameters
    {

        /// <inheritdoc />
        public MemoryCrudTestParameters() : base(new CrudMemory<TestItemBare, Guid>())
        {
            FulcrumApplicationHelper.UnitTestSetup(nameof(MemoryCrudTestParameters));
        }
    }
}
