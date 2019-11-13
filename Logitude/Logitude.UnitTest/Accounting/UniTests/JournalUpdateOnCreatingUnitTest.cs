using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class JournalUpdateOnCreatingUnitTest
    {
        [TestMethod]
        public void OnCreating_FillDefaultFieldsJIdJNumber()
        {
            int tenant = 1;
            var expcted_IdCounter = "1-IdCounter";
            var expcted_CodeCounter = 72626;
            var expcted_LogId="itzikid";
            string expcted_ObjectTableId ="expcted_ObjectTableId";
            //arrange
            var journalPM = new JournalPM()
            {
                Tenant = tenant,
                

            };

            var bAddAcitivityLog = false;


            var fakeJournalUpdateOnCreating = 
                A.Fake<JournalUpdateOnCreating>(option => option.CallsBaseMethods());
                
            A.CallTo(() => fakeJournalUpdateOnCreating.IdCounterWrapperGetNumber(tenant))
            .Returns(expcted_IdCounter);
            A.CallTo(() => fakeJournalUpdateOnCreating.CodeCounterWrapperGetNumber(tenant))
            .Returns(expcted_CodeCounter);
            A.CallTo(() => fakeJournalUpdateOnCreating.GetLogContactId(journalPM))
            .Returns(expcted_LogId);

            A.CallTo(() => fakeJournalUpdateOnCreating.GetObjectTableId(journalPM))
            .Returns(expcted_ObjectTableId);

            A.CallTo(() => fakeJournalUpdateOnCreating.AddAcitivityLog(journalPM, expcted_LogId, expcted_ObjectTableId))
            .Invokes(call => { bAddAcitivityLog = true; });
            
            
            //act
            fakeJournalUpdateOnCreating.OnCreating(journalPM, null);

            ///check
            Assert.AreEqual(expcted_IdCounter, journalPM.Id);
            Assert.AreEqual(expcted_CodeCounter.ToString(), journalPM.JournalNumber);


        }
    }
}
