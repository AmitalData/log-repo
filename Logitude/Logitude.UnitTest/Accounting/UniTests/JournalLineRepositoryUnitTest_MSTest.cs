using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Test.Infrastructure;
using Logitude.Test.Infrastructure.Factories;
using System.Collections.Generic;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// MSTest-compatible version of JournalLineRepositoryUnitTest.
    /// Migrated to MSTest - simple test with no mocking required.
    /// </summary>
    [TestClass]
    public class JournalLineRepositoryUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "InsureUsingOnlyByUpdateService")]
        public void JournalLineRepositoryAdd_UseWithoutUpdateService_ThrowExcptionInsureUsingOnlyByUpdateService()
        {
            var repo = new JournalLineRepository(
                (new AccountingFakeFactory_MSTest()).CreateFakeIAccountingContext(
                    null,
                    new List<JournalLine>(),
                    null
                ));
            var poco = new JournalLine() { Tenant = 1, JournalId = "1-1", Line = 1 };
            repo.Add(poco);
        }
    }
}


