using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.UnitTest.ItzikTest
{
    [TestClass]
    public class LinqUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = new List<int>() { 1, 2, 4, 8, 9, 57 };
            var removeList = new List<int>() { 1, 8, 57 };
            var newlist = list.Where(i => !removeList.Any(yt => i == yt));
            Assert.IsTrue(newlist.Contains(2));
            Assert.IsTrue(newlist.Contains(4));
            Assert.IsTrue(newlist.Contains(9));
            Assert.IsFalse(newlist.Contains(1));
            Assert.IsFalse(newlist.Contains(8));
            Assert.IsFalse(newlist.Contains(57));
        }


    }
}
