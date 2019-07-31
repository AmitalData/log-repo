using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
//using Logitude.Accounting.Data.Fakes;
//using System.Data.Entity;
using Logitude.Accounting.Data.EntityPOCOs;
using System.Collections.Generic;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using System.Diagnostics;
using FakeItEasy;
using Logitude.UnitTest.Utils;
using Logitude.UnitTest.Accounting;
//using Microsoft.QualityTools.Testing.Fakes;
//using Logitude.Accounting.Data.Repositories.Fakes;
//using Simplog.Global.Data.GlobalModel.Fakes;
//using Simplog.Global.Data.GlobalModel.Fakes;

namespace Logitude.UnitTest
{
    [TestClass]
    public class TestingTestingUnitTest : TestBase
    {

        [TestMethod]
        
        public void MyTestMethod()
        {
            Trace.WriteLine("its good to trace ");    
        }

        [TestMethod]
        public void TestMethod1()
        {
            var JournalId = "1-1";
            var fakeIAccountingContext = A.Fake<IAccountingContext>();
            A.CallTo(() =>fakeIAccountingContext.Journals)
                .Returns(new MockObjectSet<Journal>() { new Journal() { Id = JournalId } });
            
            var pm = new JournalPM();
            var qs = new JournalQueryService(fakeIAccountingContext);
            //var qs = new JournalRepository(1);
            var fetchPm = qs.GetSingle(JournalId, false, false);
            Assert.AreEqual(fetchPm.Id, JournalId);
        }
        [TestMethod]
        public void TestMethod2()
        {
            var JournalId = "1-1";
            var fakeIAccountingContext = A.Fake<IAccountingContext>();
            A.CallTo(() => fakeIAccountingContext.Journals)
                .Returns(new MockObjectSet<Journal>() { new Journal() { Id = JournalId } });
            
            var pm = new JournalPM();
            var qs = new JournalQueryService(fakeIAccountingContext);
            //var qs = new JournalRepository(1);
            var fetchPm = qs.GetSingle(JournalId, false, false);
            Assert.AreEqual(fetchPm.Id, JournalId);
        }
        [TestMethod]
        [Ignore()] //"there is a problem with this test"
        public void JournalUpdateServiceUpdate_UsingShimUsingShimUsingShimUsingShimUsingShimUsingShim()
        {
            var JournalId = "1-1";
            var GLAccountId = "1-1";
            int tenant = 1;
            IAccountingContext fakeIAccountingContext=
            (new AccountingFakeFactory()).CreateFakeIAccountingContext(
            new List<Journal>(){new Journal() { Id = JournalId, Tenant = tenant }}, 
            null,
            new List<GLAccount>(){new GLAccount() { Id = GLAccountId, Tenant = tenant }}
            );

            
            

            //ds.
            //using (ShimsContext.Create())
            try
            {
                var pm = new JournalPM();
                pm.Id = JournalId;
                pm.ExternalNo = "3333";
                pm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                //var us = new Logitude.Accounting.BL.EntityUpdateServices.Fakes.ShimJournalUpdateService();
                //us.up

                var fakeJournalUpdateService = (new AccountingFakeFactory()).CreateFakeJournalUpdateService(tenant, fakeIAccountingContext, true);
                //var us = new JournalUpdateService(fakeIAccountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                fakeJournalUpdateService.Update(pm, true);

                var qs = new JournalQueryService(fakeIAccountingContext);
                //var qs = new JournalRepository(1);
                var fetchPm = qs.GetSingle(JournalId, false, false);
                Assert.AreEqual(fetchPm.Id, JournalId);
                Assert.AreEqual(fetchPm.Id, JournalId);
            }
            finally
            {
            }


        }

   

        [TestMethod]
        [Ignore()] //"there is a problem with this test"
        public void JournalUpdateServiceUpdate_UsingShimUsingShimUsingShimUsingShimUsingShimUsingShim1()
        {
            var JournalId = "1-1";
            var GLAccountId = "1-1";
            var tenant = 1;
            IAccountingContext fakeIAccountingContext =
            (new AccountingFakeFactory()).CreateFakeIAccountingContext(
            new List<Journal >(){new Journal() { Id = JournalId, Tenant = tenant }},
            null,
            new List<GLAccount> () { new GLAccount() { Id = GLAccountId, Tenant = tenant }}
            );

            //ds.
            try//using (ShimsContext.Create())
            {
                var pm = new JournalPM();
                pm.Id = JournalId;
                pm.ExternalNo = "3333";
                pm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                //var us = new Logitude.Accounting.BL.EntityUpdateServices.Fakes.ShimJournalUpdateService();
                //us.up
                //ShimJournalRepository.AllInstances.contextGet = (myRepo) =>
                //{
                //    myRepo = myRepo;
                //    return (stubIAccountingContext as IAccountingContext);
                //};

                //ShimGlobalContext.GetContext = () =>
                //{
                //    return stubGlobalContext;
                //};
                //Logitude.Accounting.BL.EntityUpdateServices.Fakes.ShimJournalUpdateService.AllInstances.TraceJournalPMJournalString = (@this, a, b, c) =>
                //{
                //    Trace.Write("Override JournalUpdateService.AllInstances.TraceJournalPMJournalString !!!!");
                //};
                var us = //new JournalUpdateService(stubIAccountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                    (new AccountingFakeFactory()).CreateFakeJournalUpdateService(tenant, fakeIAccountingContext, true);
                us.Update(pm, true);

                var qs = new JournalQueryService(fakeIAccountingContext);
                //var qs = new JournalRepository(1);
                var fetchPm = qs.GetSingle(JournalId, false, false);
                Assert.AreEqual(fetchPm.Id, JournalId);
                Assert.AreEqual(fetchPm.Id, JournalId);
                Assert.AreEqual(fetchPm.ExternalNo, "3333");
            }
            finally { }



        }
        

        
    }
}
