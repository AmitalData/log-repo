using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.UnitTest.Utils;
using System.Collections.Generic;
using Logitude.Test.Infrastructure;
using Logitude.Test.Infrastructure.Factories;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// Refactored version of JournalRepositoryUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class JournalRepositoryUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "")]
        public void JournalRepositoryAdd_TrowExceptionInsureUsingOnlyByUpdateService()
        {
            string JournalId = "1-1";
            
            var repo = new JournalRepository(
                (new AccountingFakeFactory_MSTest()).CreateFakeIAccountingContext(
                    new List<Journal>() { new Journal() { Id = JournalId } },
                    null,
                    null
                ));
            var pm = new JournalPM();
            var poco = new Journal() { Tenant = 1 };
            repo.Add(poco);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "")]
        public void JournalRepositoryUpdate_TrowExceptionInsureUsingOnlyByUpdateService()
        {
            string JournalId = "1-1";
            
            var repo = new JournalRepository(
                (new AccountingFakeFactory_MSTest()).CreateFakeIAccountingContext(
                    new List<Journal>() { new Journal() { Id = JournalId } },
                    null,
                    null)
                );
            var pm = new JournalPM();
            var poco = new Journal() { Tenant = 1 };
            repo.Update(poco);
        }
    }
}

