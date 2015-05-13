using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SingletonTest.Test
{
    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void TestLazySingleton()
        {
            var singletonThing = SingletonThing.Instance;

            Assert.AreEqual("Chris", singletonThing.Name1);
            Assert.AreEqual("Ballance", singletonThing.Name2);

            singletonThing.Name1 = "Not Chris";
            singletonThing.Name2 = "Not Ballance";

            var singletonThingShouldBeSame = SingletonThing.Instance;

            Assert.AreNotEqual("Chris", singletonThingShouldBeSame.Name1);
            Assert.AreNotEqual("Ballance", singletonThingShouldBeSame.Name2);

            Assert.AreSame(singletonThing, singletonThingShouldBeSame);
            Assert.AreSame(singletonThing.Name1, singletonThingShouldBeSame.Name1);
            Assert.AreSame(singletonThing.Name2, singletonThingShouldBeSame.Name2);


            GCHandle objHandle = GCHandle.Alloc(singletonThing.Name1, GCHandleType.Normal);
            Int64 address = GCHandle.ToIntPtr(objHandle).ToInt64(); 
            Console.WriteLine("Initial address: {0} ", address.ToString("X"));
           

            var threadState = new ThreadArgs();
            ThreadPool.QueueUserWorkItem(ThreadWorker, threadState);

            var singletonAfterThreadActivity = SingletonThing.Instance;

            while (!threadState.IsCompleted) { }

            Assert.AreEqual("ThreadChris1_1", singletonAfterThreadActivity.Name1);
            Assert.AreEqual("ThreadChris1_2", singletonAfterThreadActivity.Name2);

            var threadState2 = new ThreadArgs();
            ThreadPool.QueueUserWorkItem(ThreadWorker2, threadState2);

            while (!threadState2.IsCompleted) { }

            Assert.AreEqual("ThreadChris2_1", singletonAfterThreadActivity.Name1);
            Assert.AreEqual("ThreadChris2_2", singletonAfterThreadActivity.Name2);

            var threadState3 = new ThreadArgs();
            ThreadPool.QueueUserWorkItem(ThreadWorker3, threadState3);

            while (!threadState3.IsCompleted) { }

            Assert.AreEqual("ThreadChris3_1", singletonAfterThreadActivity.Name1);
            Assert.AreEqual("ThreadChris3_2", singletonAfterThreadActivity.Name2);

            GCHandle objHandle2 = GCHandle.Alloc(singletonAfterThreadActivity, GCHandleType.Normal);
            Int64 address2 = GCHandle.ToIntPtr(objHandle2).ToInt64();
            Console.WriteLine("Later address: {0} ", address2.ToString("x"));
        }

        public void ThreadWorker(object state)
        {
            var singleton = SingletonThing.Instance;
            singleton.Name1 = "ThreadChris1_1";
            singleton.Name2 = "ThreadChris1_2";

            var threadState = (ThreadArgs)state;
            Thread.Sleep(1001);

            threadState.IsCompleted = true;
        }

        public void ThreadWorker2(object state)
        {
            var singleton = SingletonThing.Instance;
            singleton.Name1 = "ThreadChris2_1";
            singleton.Name2 = "ThreadChris2_2";

            var threadState = (ThreadArgs)state;
            Thread.Sleep(1002);

            threadState.IsCompleted = true;
        }

        public void ThreadWorker3(object state)
        {
            var singleton = SingletonThing.Instance;
            singleton.Name1 = "ThreadChris3_1";
            singleton.Name2 = "ThreadChris3_2";

            var threadState = (ThreadArgs)state;
            Thread.Sleep(1003);

            threadState.IsCompleted = true;
        }
    }
}
