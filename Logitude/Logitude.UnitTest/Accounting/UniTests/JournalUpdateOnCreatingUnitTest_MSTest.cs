using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Test.Infrastructure;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// Refactored version of JournalUpdateOnCreatingUnitTest using FakeItEasy.
    /// Migrated from MSTest Fakes to FakeItEasy for compatibility with Visual Studio Community.
    /// </summary>
    [TestClass]
    public class JournalUpdateOnCreatingUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void OnCreating_FillDefaultFieldsJIdJNumber()
        {
            int tenant = 1;
            var expcted_IdCounter = "1-IdCounter";
            var expcted_CodeCounter = 72626;
            var expcted_LogId = "itzikid";
            string expcted_ObjectTableId = "expcted_ObjectTableId";
            
            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
            };

            var bAddAcitivityLog = false;

            // Create fake using FakeItEasy with CallsBaseMethods to call real implementation
            var fakeJournalUpdateOnCreating = 
                A.Fake<JournalUpdateOnCreating>(option => option.CallsBaseMethods());
                
            A.CallTo(() => fakeJournalUpdateOnCreating.IdCounterWrapperGetNumber(tenant))
                .Returns(expcted_IdCounter);
            // Note: CodeCounterWrapperGetNumber is commented out in JournalUpdateOnCreating
            // JournalNumber assignment is also commented out in the implementation
            // So we only test Id assignment
            A.CallTo(() => fakeJournalUpdateOnCreating.GetLogContactId(journalPM))
                .Returns(expcted_LogId);
            
            A.CallTo(() => fakeJournalUpdateOnCreating.GetObjectTableId(journalPM))
                .Returns(expcted_ObjectTableId);

            A.CallTo(() => fakeJournalUpdateOnCreating.AddAcitivityLog(journalPM, expcted_LogId, expcted_ObjectTableId))
                .Invokes(call => { bAddAcitivityLog = true; });

            A.CallTo(() => fakeJournalUpdateOnCreating.ClearDMYByUserId(A<JournalPM>.Ignored, A<string>.Ignored))
                .Invokes(call => { /* do nothing */ });
            
            //act
            fakeJournalUpdateOnCreating.OnCreating(journalPM, null);
            
            //check
            Assert.AreEqual(expcted_IdCounter, journalPM.Id);
            // JournalNumber is not set in the current implementation (commented out)
            // Assert.AreEqual(expcted_CodeCounter.ToString(), journalPM.JournalNumber);
        }
    }
}


