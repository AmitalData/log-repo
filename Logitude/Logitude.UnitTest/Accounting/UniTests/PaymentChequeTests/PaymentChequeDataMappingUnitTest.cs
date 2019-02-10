using FakeItEasy;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests.PaymentChequeTests
{
    [TestClass]
     public class PaymentChequeDataMappingUnitTest
    {

        [TestMethod]
        public void PMToPOCO_CheckCustomMappedFieldsMatchExpected_Success()
        {

            string  expected_Id = "111";
             int expected_tenant = 1;
            string Expected_SearchFields = "search";
      
            PaymentChequePM entityPM = new PaymentChequePM()
            {
             
               Id = expected_Id,
               Tenant = expected_tenant,
               SearchFields = Expected_SearchFields,
               ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert
            };
            PaymentCheque entityPOCO = new PaymentCheque();
            var paymentChequeCustomDataMapping = A.Fake<PaymentChequeCustomDataMapping>(option => option.CallsBaseMethods());
            paymentChequeCustomDataMapping.PMToPOCO(entityPM, entityPOCO, new List<PaymentChequeDataMapping.POCOPropertyNames>());

            Assert.AreEqual(expected_Id, entityPOCO.Id, "Id not matches expected Id");
            Assert.AreEqual(expected_tenant, entityPOCO.Tenant, "Tenant not matches expected tenant");
            Assert.AreEqual(Expected_SearchFields, entityPOCO.SearchFields, "search fields not matches expected search fields ");


        }

        [TestMethod]
        public void POCOToPM_CheckCustomMappedFieldsMatchExpected_Success()
        {
           

        }



    }
}
