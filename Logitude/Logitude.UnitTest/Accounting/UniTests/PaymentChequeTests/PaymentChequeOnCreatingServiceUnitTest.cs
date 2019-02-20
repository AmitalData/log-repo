using FakeItEasy;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests.PaymentChequeTests
{

    [TestClass]
    public class PaymentChequeOnCreatingServiceUnitTest : TestBase
    {
        [TestMethod]
        public void OnCreating_IdMatchesExpected_Success() 
        {
            ContactPM loggedcontact = GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            DateTime expectedDateTime = DateTime.Now;
            string expectedIdCounter = "myNewIdCounter";
            string expectedCodeCounter = "1000";

            PaymentChequePM entityPM = new PaymentChequePM()
            {
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };


            var paymentChequeOnCreatingService = A.Fake<PaymentChequeOnCreatingService>(option => option.CallsBaseMethods());
           // A.CallTo(() => paymentChequeOnCreatingService.GetLogContactId(entityPM.Tenant)).Returns(expectedLoggedUserId);
            A.CallTo(() => paymentChequeOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => paymentChequeOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            A.CallTo(() => paymentChequeOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);

            // Act
            paymentChequeOnCreatingService.OnCreating(entityPM);

            // Assert
            Assert.AreEqual(expectedIdCounter, entityPM.Id, "Id not matches expected #DP01");

        }


        [TestMethod]
        public void OnCreating_NumberMatchesExpected_Success()
        {
            // Arrang
            ContactPM loggedcontact = GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            DateTime expectedDateTime = DateTime.Now;
            string expectedIdCounter = "myNewIdCounter";
            string expectedCodeCounter = "1000";

            PaymentChequePM entityPM = new PaymentChequePM()
            {
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };


            var paymentChequeOnCreatingService = A.Fake<PaymentChequeOnCreatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => paymentChequeOnCreatingService.GetLogContactId(entityPM.Tenant)).Returns(expectedLoggedUserId);
            A.CallTo(() => paymentChequeOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => paymentChequeOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            A.CallTo(() => paymentChequeOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);

            // Act
            paymentChequeOnCreatingService.OnCreating(entityPM);
            entityPM.InternalNumber = "11";
            entityPM.Id = "22";
            // Assert

      

            Aggregate(
        () => Assert.AreEqual(expectedCodeCounter, entityPM.InternalNumber, "Number not matches expected"),
        () => Assert.AreEqual(expectedIdCounter, entityPM.Id, "Id not matches expected"),
          () => Assert.AreEqual(expectedLoggedUserId, entityPM.UpdatedByUserId, "Id not matches expected"));

           

        }

        [TestMethod]
        public void OnCreating_CreatePaymentLine_Success()
        {
            // Arrang
            ContactPM loggedcontact = GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            DateTime expectedDateTime = DateTime.Now;
            string expectedIdCounter = "myNewIdCounter";
            string expectedCodeCounter = "1000";

            PaymentChequePM entityPM = new PaymentChequePM()
            {
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Notes = "1254",
            };


            var paymentChequeOnCreatingService = A.Fake<PaymentChequeOnCreatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => paymentChequeOnCreatingService.GetLogContactId(entityPM.Tenant)).Returns(expectedLoggedUserId);
            A.CallTo(() => paymentChequeOnCreatingService.GetCurrentDateTime(entityPM.Tenant)).Returns(expectedDateTime);
            A.CallTo(() => paymentChequeOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            A.CallTo(() => paymentChequeOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);

            // Act
            paymentChequeOnCreatingService.OnCreating(entityPM);
            var chequeLine = entityPM.PaymentChequeLines.Count();
       
            // Assert
            Assert.AreNotEqual(chequeLine,0, "Payment cheque has no lines");

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
