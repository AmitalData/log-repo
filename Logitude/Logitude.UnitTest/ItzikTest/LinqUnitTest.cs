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



        [TestMethod]
        public void TestLetAsLeftJoin()
        {
            var list = new List<MyClass>() {

                new MyClass () { accid="1"},
                new MyClass () { accid="2"},
                new MyClass () { accid="3"},
                new MyClass () { accid="4"},

            };
            var leftjoinList = new List<MyClass>() {

                new MyClass () { accid="1" , currName="USD"},
                new MyClass () { accid="2", currName="NIS"},
                new MyClass () { accid="3", currName="USD"},
                new MyClass () { accid="5"},

            };
            var newlist = (from a in list
                           let cur = leftjoinList.FirstOrDefault(r => r.accid == a.accid)
                           select new MyClass()
                           {
                               accid = a.accid,
                               currName = cur?.currName
                           }
                           );
            var l = newlist.ToList();
            Assert.IsNotNull(newlist.First(r => r.accid == "1" && r.currName == "USD"));
            Assert.IsNotNull(newlist.First(r => r.accid == "2" && r.currName == "NIS"));
            Assert.IsNotNull(newlist.First(r => r.accid == "3" && r.currName == "USD"));
            Assert.IsNotNull(newlist.First(r => r.accid == "4" && string.IsNullOrEmpty(r.currName)));


        }


    }

    class MyClass
    {
        public string accid { get; set; }

        public int total { get; set; }
        public string currName{ get; set; }
    }
    
    
}
