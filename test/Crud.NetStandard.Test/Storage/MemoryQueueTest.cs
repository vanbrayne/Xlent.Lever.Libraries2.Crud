using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xlent.Lever.Libraries2.Core.Queue.Logic;
using Xlent.Lever.Libraries2.Core.Queue.Model;
using Xlent.Lever.Libraries2.Crud.Test.NuGet;
using Xlent.Lever.Libraries2.Crud.Test.NuGet.Model;

namespace Xlent.Lever.Libraries2.Crud.NetFramework.Test.Crud.Storage
{
    [TestClass]
    public class MemoryQueueTest : TestIQueue
    {
        private MemoryQueue<TestItemBare> _queue;

        [TestInitialize]
        public void Inititalize()
        {
            _queue = new MemoryQueue<TestItemBare>("test-queue");
        }

        protected override ICompleteQueue<TestItemBare> Queue => _queue;
    }
}
