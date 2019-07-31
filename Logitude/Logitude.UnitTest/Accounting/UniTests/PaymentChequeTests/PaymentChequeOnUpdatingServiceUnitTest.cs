using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.EntityPOCOs;
using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;

namespace Logitude.UnitTest.Accounting.UniTests.PaymentChequeTests
{
    [TestClass]
    public class PaymentChequeOnUpdatingServiceUnitTest :TestBase
    {

        [TestMethod]
        public void OnUpdating_PaymentChequeUpdate_Success()
        {
            int tenant = 1;
            var expcted_IdCounter = "1-1";
            var expcted_Number = "1000";
            var expcted_LogId = GetLoggedContactInstance().Id;
            
            //arrange
            var PaymentChequePM = new PaymentChequePM()
            {
                Tenant = tenant,
                CreatedByUserId = expcted_LogId,
                Id = expcted_IdCounter,
                InternalNumber = expcted_Number,
            };

           

            var PaymentChequeUpdateService = A.Fake<PaymentChequeOnUpdatingService>(option => option.CallsBaseMethods());

            A.CallTo(() => PaymentChequeUpdateService.GetLogContactId(PaymentChequePM.Tenant)).Returns(expcted_LogId);
            A.CallTo(() => PaymentChequeUpdateService.GetCurrentDateTime(PaymentChequePM.Tenant)).Returns(DateTime.Now);

            

            //act
            PaymentChequeUpdateService.OnUpdating(PaymentChequePM,null);
     
            ///check
            Assert.AreEqual(expcted_IdCounter, PaymentChequePM.Id);
            Assert.AreEqual(expcted_Number.ToString(), PaymentChequePM.InternalNumber,"does not exist");


        }



        [TestMethod]
        public void OnUpdating_CreatingJournal_Success()
        {
            int tenant = 1;
            var expcted_IdCounter = "1-1";
            var expcted_Number = "1000";
            var expcted_LogId = GetLoggedContactInstance().Id;

            //arrange
            var PaymentChequePM = new PaymentChequePM()
            {
                Tenant = tenant,
                CreatedByUserId = expcted_LogId,
                Id = expcted_IdCounter,
                InternalNumber = expcted_Number,
                ChequeNumber ="11",
                UpdatedByUserId = expcted_LogId,
                PaymentChequeStatusCode="2",
                BankAccountId ="1-1"

            };

            JournalPM journalPM = new JournalPM()
            {
                Tenant = 1,
                AccountingEntityReference = PaymentChequePM.ChequeNumber,           
                UpdatedByUserId = PaymentChequePM.UpdatedByUserId,
            
            };

            //BankAccountPM bankAccountPM = new BankAccountPM()
            //{
            //    Tenant = 1,
            //   ChequeCounter =1,
            //    BankId = PaymentChequePM.BankAccountId,
            //    Id ="1-1"
            //};


            var PaymentChequeUpdateService = A.Fake<PaymentChequeOnUpdatingService>(option => option.CallsBaseMethods());

           // A.CallTo(() => PaymentChequeUpdateService.GetLogContactId(PaymentChequePM.Tenant)).Returns(expcted_LogId);
            A.CallTo(() => PaymentChequeUpdateService.GetCurrentDateTime(PaymentChequePM.Tenant)).Returns(DateTime.Now);
            A.CallTo(() => PaymentChequeUpdateService.CreateJournalPM(PaymentChequePM)).Returns(journalPM);
       //     A.CallTo(() => PaymentChequeUpdateService.GetSingleBankAccountPM(PaymentChequePM)).Returns(bankAccountPM);

            //act
           //PaymentChequeUpdateService.OnUpdating(PaymentChequePM, null);

            ///check
            //Assert.AreEqual(journalPM.TypeCode, "0", "does not exist");
            //Assert.AreEqual(expcted_IdCounter, PaymentChequePM.Id);
            //Assert.AreEqual(expcted_Number.ToString(), PaymentChequePM.InternalNumber, "does not exist");


        }



        private ContactPM GetLoggedContactInstance()
        {
            string expectedLoggedUserId = "myUser";
            ContactPM loggedcontact = new ContactPM()
            {
                Id = expectedLoggedUserId,
                DontShowLocal = true,
            };

            return loggedcontact;
        }

    }
}
