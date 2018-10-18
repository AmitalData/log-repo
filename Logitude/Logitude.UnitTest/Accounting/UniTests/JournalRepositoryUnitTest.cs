using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.Data.Repositories;
//using Logitude.Accounting.Data.Fakes;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.UnitTest.Utils;
using System.Collections.Generic;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class JournalRepositoryUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception),"")]
        public void JournalRepositoryAdd_TrowExceptionInsureUsingOnlyByUpdateService()
        {
            
            string JournalId ="1-1";
            //var stubIAccountingContext = new StubIAccountingContext()
            //{
            //    JournalsGet = () =>
            //    {
            //        var mock= new MockObjectSet<Journal>() { new Journal() { Id = JournalId } };
            //        return mock;
            //    }
            //};
            var repo = new JournalRepository((new AccountingFakeFactory()).CreateFakeIAccountingContext(
                new List<Journal>(){new Journal() { Id = JournalId }}
                , null,null
                ));
            var pm = new JournalPM();
            var poco = new Journal() { Tenant =1} ;
            repo.Add(poco);
            //var qs = new JournalQueryService(stubIAccountingContext);
            //var qs = new JournalRepository(1);
            //var fetchPm=qs.GetSingle(JournalId, false, false);
            //Assert.AreEqual(fetchPm.Id, JournalId);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "")]
        public void JournalRepositoryUpdate_TrowExceptionInsureUsingOnlyByUpdateService()
        {

            string JournalId = "1-1";
            //var stubIAccountingContext = new StubIAccountingContext()
            //{
            //    JournalsGet = () => { return new MockObjectSet<Journal>() { new Journal() { Id = JournalId } }; }
            //};
            var repo = new JournalRepository(
                (new AccountingFakeFactory()).CreateFakeIAccountingContext(
                new List<Journal>(){new Journal() { Id = JournalId }},
                null,null )
                );
            var pm = new JournalPM();
            var poco = new Journal() { Tenant = 1 };
            repo.Update(poco);
            //var qs = new JournalQueryService(stubIAccountingContext);
            //var qs = new JournalRepository(1);
            //var fetchPm=qs.GetSingle(JournalId, false, false);
            //Assert.AreEqual(fetchPm.Id, JournalId);
        }
    }
}
