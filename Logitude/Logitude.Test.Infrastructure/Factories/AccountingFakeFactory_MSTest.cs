using System;
using System.Collections.Generic;
using System.Diagnostics;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Test.Infrastructure.Container;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.Helpers;
using FakeItEasy;

namespace Logitude.Test.Infrastructure.Factories
{
    /// <summary>
    /// Refactored version of AccountingFakeFactory using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    public class AccountingFakeFactory_MSTest
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance with the specified container.
        /// </summary>
        public AccountingFakeFactory_MSTest(IUnityContainer container = null)
        {
            _container = container ?? ContainerAccessor.Container;
        }

        /// <summary>
        /// Creates a fake IAccountingContext using FakeItEasy.
        /// </summary>
        public IAccountingContext CreateFakeIAccountingContext(
            List<Journal> pocoJournalList,
            List<JournalLine> pocoJournalLines,
            List<GLAccount> pocoGLAccountList)
        {
            pocoJournalList = pocoJournalList ?? new List<Journal>();
            pocoJournalLines = pocoJournalLines ?? new List<JournalLine>();
            pocoGLAccountList = pocoGLAccountList ?? new List<GLAccount>();

            // Create fake for IAccountingContext using FakeItEasy
            var fakeIAccountingContext = A.Fake<IAccountingContext>();
            
            // Configure properties to return MockObjectSet instances
            A.CallTo(() => fakeIAccountingContext.Journals).Returns(new MockObjectSet<Journal>(pocoJournalList));
            A.CallTo(() => fakeIAccountingContext.JournalLines).Returns(new MockObjectSet<JournalLine>(pocoJournalLines));
            A.CallTo(() => fakeIAccountingContext.GLAccounts).Returns(new MockObjectSet<GLAccount>(pocoGLAccountList));
            
            var journalActionTypeList = new List<JournalActionType>()
            {
                new JournalActionType() { Tenant=1, Code="1", Id="1", EnglishName ="Credit" },
                new JournalActionType() { Tenant=1, Code="2", Id="2", EnglishName ="Debit " },
                new JournalActionType() { Tenant=1, Code="3", Id="3", EnglishName ="Debit And Credit" },
            };
            A.CallTo(() => fakeIAccountingContext.JournalActionTypes).Returns(new MockObjectSet<JournalActionType>(journalActionTypeList));
            
            return fakeIAccountingContext;
        }

        /// <summary>
        /// Creates a fake JournalUpdateService using MSTest fakes.
        /// Note: Requires Logitude.Accounting.BL.fakes to be configured.
        /// This is a complex factory method that sets up many dependencies.
        /// </summary>
        public JournalUpdateService CreateFakeJournalUpdateService(
            int tenant, 
            IAccountingContext fakeIAccountingContext, 
            bool suppressValidate = false)
        {
#if FAKES_SUPPORTED
            // This method requires extensive setup with multiple fakes:
            // - IObjectTableRepository
            // - IIdCounter
            // - IActivityLogger
            // - ICodeCounter
            // - IContactRepository
            // - ICurrencyQuery
            // - IGLAccountQueryService
            // - JournalUpdateService (with CallsBaseMethods equivalent)
            
            // MSTest fakes approach for JournalUpdateService:
            // 1. Create shims for all dependencies
            // 2. Register them in the container
            // 3. Create a shim for JournalUpdateService with constructor arguments
            // 4. Configure method shims for Trace and Validate if needed
            
            // Example (requires Logitude.Accounting.BL.fakes):
            // using (ShimsContext.Create())
            // {
            //     var shim = new Fakes.ShimJournalUpdateService();
            //     // Configure constructor
            //     // Configure method behaviors
            //     return shim.Instance;
            // }
            
            throw new NotImplementedException(
                "MSTest fakes support requires Logitude.Accounting.BL.fakes and related .fakes files to be configured and built. " +
                "This will be implemented when .fakes files are properly set up.");
#else
            throw new NotImplementedException(
                "MSTest fakes support requires .fakes files to be configured and built.");
#endif
        }
    }
}


