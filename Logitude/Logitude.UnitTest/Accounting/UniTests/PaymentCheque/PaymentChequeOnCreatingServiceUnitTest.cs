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

namespace Logitude.UnitTest.Accounting.UniTests.PaymentCheque
{

    [TestClass]
    public class PaymentChequeOnCreatingServiceUnitTest
    {
        [TestMethod]
        public void OnCreating_IdMatchesExpected_Success() 
        {
            ContactPM loggedcontact = GetLoggedContactInstance();
            string expectedLoggedUserId = loggedcontact.Id;
            DateTime expectedDateTime = DateTime.Now;
            string expectedIdCounter = "myNewIdCounter";
            int expectedCodeCounter = 1000;

            PaymentChequePM entityPM = new PaymentChequePM()
            {
                Tenant = 1,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };


            var paymentChequeOnCreatingService = A.Fake<PaymentChequeOnCreatingService>(option => option.CallsBaseMethods());
            A.CallTo(() => paymentChequeOnCreatingService.GetLogContactId(entityPM.Tenant)).Returns(expectedLoggedUserId);
            A.CallTo(() => paymentChequeOnCreatingService.GetCurrentDateTime()).Returns(expectedDateTime);
            A.CallTo(() => paymentChequeOnCreatingService.IdCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedIdCounter);
            A.CallTo(() => paymentChequeOnCreatingService.CodeCounterWrapperGetNumber(entityPM.Tenant)).Returns(expectedCodeCounter);

            // Act
            paymentChequeOnCreatingService.OnCreating(entityPM);

            // Assert
            Assert.AreEqual(expectedIdCounter, entityPM.Id, "Id not matches expected #DP01");

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
