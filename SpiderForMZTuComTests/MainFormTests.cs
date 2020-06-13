using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpiderForMZTuCom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderForMZTuCom.Tests
{
    [TestClass()]
    public class MainFormTests
    {
        [TestMethod()]
        public void TestTest()
        {
            var slim = new System.Threading.SemaphoreSlim(0, 2);
            slim.Wait();
            slim.Wait();
            slim.Wait();


        }
    }
}